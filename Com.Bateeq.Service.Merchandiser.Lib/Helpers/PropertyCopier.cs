namespace Com.Bateeq.Service.Merchandiser.Lib.Helpers
{
    public class PropertyCopier<OriginClass, DestinationClass>
        where OriginClass : class
        where DestinationClass : class
    {
        public static void Copy(OriginClass origin, DestinationClass destination)
        {
            var originProperties = origin.GetType().GetProperties();
            var destinationProperties = destination.GetType().GetProperties();
            foreach (var originProperty in originProperties)
            {
                foreach (var destinationProperty in destinationProperties)
                {
                    if (originProperty.Name == destinationProperty.Name && originProperty.PropertyType == destinationProperty.PropertyType)
                    {
                        destinationProperty.SetValue(destination, originProperty.GetValue(origin));
                        break;
                    }
                }
            }
        }
    }
}
