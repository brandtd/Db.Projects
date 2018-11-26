#region MIT License (c) 2018 Dan Brandt

// Copyright 2018 Dan Brandt
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion MIT License (c) 2018 Dan Brandt

using DotSpatial.Positioning;
using MapControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Db.Controls.Test
{
    /// <summary>Data context to use for <see cref="DraggableMapControlTest" />.</summary>
    public class DraggableMapControlTestDataContext : PropertyChangedBase, INotifyPropertyChanged
    {
        public DraggableMapControlTestDataContext()
        {
            MapItems = new ReadOnlyObservableCollection<UIElement>(_mapItems);
            _center = _centers[_centerIndex];
            _mapItems.Add(new TestMapItem());
            testLoop();
        }

        /// <summary>Center location of map.</summary>
        public Position MapCenter { get => _center; set => SetProperty(ref _center, value); }

        /// <summary>Items to place in map.</summary>
        public ReadOnlyObservableCollection<UIElement> MapItems { get; }

        /// <summary>Projection type to use.</summary>
        public MapProjection Projection { get; } = new WebMercatorProjection();

        /// <summary>Whether to show map layer description.</summary>
        public bool ShowDescription { get => _showDescription; set => SetProperty(ref _showDescription, value); }

        /// <summary>Whether to show extra map info like the layer selector and mouse location.</summary>
        public bool ShowExtras { get => _showExtras; set => SetProperty(ref _showExtras, value); }

        /// <summary>Map's zoom level.</summary>
        public double Zoom { get => _zoom; set => SetProperty(ref _zoom, value); }

        /// <summary>The underlaying map layer.</summary>
        public UIElement MapLayer => _mapLayers.MapLayer;

        /// <summary>Underlaying map layer.</summary>
        public string MapLayerName => _mapLayers.MapLayerName;

        /// <summary>All map layer names.</summary>
        public IEnumerable<string> MapLayerNames => _mapLayers.MapLayerNames;

        private readonly Position[] _centers = new[]
        {
            new Position(new Latitude(47.639666), new Longitude(-122.128245)),
            new Position(new Latitude(36.1070), new Longitude(-112.1130)),
        };

        private readonly ObservableCollection<UIElement> _mapItems = new ObservableCollection<UIElement>();
        private readonly DraggableMapControlMapLayers _mapLayers = new DraggableMapControlMapLayers();
        private Position _center = Position.Invalid;
        private int _centerIndex = 0;
        private bool _showDescription = true;
        private bool _showExtras = true;
        private double _zoom = 10.0;

        private async void testLoop()
        {
            for (int i = 0; i < 100; ++i)
            {
                for (int j = 0; j < 30; ++j)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(30));
                    Zoom += 0.1;
                }
                for (int j = 0; j < 30; ++j)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(30));
                    Zoom -= 0.1;
                }

                _centerIndex = (_centerIndex + 1) % _centers.Length;
                MapCenter = _centers[_centerIndex];

                await Task.Delay(TimeSpan.FromSeconds(1));
                ShowExtras = !ShowExtras;
            }
        }
    }
}