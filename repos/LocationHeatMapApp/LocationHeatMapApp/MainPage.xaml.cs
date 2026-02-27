using LocationHeatMapApp.Models;
using LocationHeatMapApp.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace LocationHeatMapApp;

public partial class MainPage : ContentPage
{
    private readonly LocationDb _db;

    private bool _tracking;
    private CancellationTokenSource? _cts;

    private const int SaveIntervalSeconds = 5;
    private const int MaxPointsToRender = 600;

    // ✅ Needed for Shell/DataTemplate creation
    public MainPage() : this(IPlatformApplication.Current.Services.GetService<LocationDb>()!)
    {
    }

    // ✅ Real DI constructor
    public MainPage(LocationDb db)
    {
        InitializeComponent();
        _db = db;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _db.InitAsync();
        await EnsureLocationPermissionAsync();

        // Guard: Map may not be ready instantly on some platforms
        await RefreshHeatAsync(centerOnLatest: true);
    }

    private async Task EnsureLocationPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
            await DisplayAlert("Permission needed", "Location permission is required.", "OK");
    }

    private void OnStartStopClicked(object sender, EventArgs e)
    {
        if (_tracking)
        {
            StopTracking();
            return;
        }

        StartTracking();
    }

    private void StartTracking()
    {
        _tracking = true;
        StartStopButton.Text = "Stop";

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        // ✅ Do not await here (keeps UI responsive)
        _ = TrackLoopAsync(_cts.Token);
    }

    private void StopTracking()
    {
        _tracking = false;
        StartStopButton.Text = "Start";
        _cts?.Cancel();
    }

    private async Task TrackLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var loc = await Geolocation.GetLocationAsync(request, token);

                if (loc != null)
                {
                    await _db.InsertAsync(new LocationPoint
                    {
                        Latitude = loc.Latitude,
                        Longitude = loc.Longitude,
                        TimestampUtc = DateTime.UtcNow
                    });

                    // ✅ UI update on main thread
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        await RefreshHeatAsync(centerOnLatest: true);
                    });
                }
            }
            catch
            {
                // keep loop alive (time-crunch friendly)
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(SaveIntervalSeconds), token);
            }
            catch
            {
                // ignored
            }
        }
    }

    private async void OnClearClicked(object sender, EventArgs e)
    {
        await _db.ClearAsync();

        if (HeatMap is not null)
            HeatMap.MapElements.Clear();

        await DisplayAlert("Cleared", "Saved locations cleared.", "OK");
    }

    private async Task RefreshHeatAsync(bool centerOnLatest)
    {
        if (HeatMap is null)
            return;

        var points = await _db.GetRecentAsync(MaxPointsToRender);
        points.Reverse();

        HeatMap.MapElements.Clear();

        if (points.Count == 0)
            return;

        var latest = points[^1];
        if (centerOnLatest)
        {
            HeatMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                new Location(latest.Latitude, latest.Longitude),
                Distance.FromKilometers(2)));
        }

        foreach (var p in points)
        {
            var center = new Location(p.Latitude, p.Longitude);

            // Outer "hot" glow
            HeatMap.MapElements.Add(new Circle
            {
                Center = center,
                Radius = new Distance(35),
                StrokeWidth = 0,
                FillColor = Color.FromRgba(255, 0, 0, 40)
            });

            // Inner "cool" core
            HeatMap.MapElements.Add(new Circle
            {
                Center = center,
                Radius = new Distance(18),
                StrokeWidth = 0,
                FillColor = Color.FromRgba(0, 70, 255, 70)
            });
        }
    }
}