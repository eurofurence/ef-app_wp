using Windows.UI.Xaml;
using Eurofurence.Companion.DataModel;
using Eurofurence.Companion.DataModel.Api;

namespace Eurofurence.Companion.ViewModel.Local.Entity
{
    public class MapEntryViewModel : EntityBase
    {
        public MapEntry Entity { get; }
        public MapViewModel Map { get; set; }

        public double Radius => (Map?.Entity.Image?.Height * (Entity.RelativeTapRadius) ?? 25);
        public double Diameter => Radius*2;

        public Thickness CenterShiftMargin => new Thickness(0-Radius, 0-Radius, Radius, Radius);

        public double X => Map?.Entity.Image?.Width * (Entity.RelativeX) ?? 0;
        public double Y => Map?.Entity.Image?.Height * (Entity.RelativeY) ?? 0;

        public MapEntryViewModel(MapEntry entity)
        {
            Entity = entity;
            InitializeDispatcherFromCurrentThread();
        }
    }
}