using Abwaab.Application.Common.Enums;

namespace Abwaab.Application.Common.Mappings
{
    public static class MediaTypesMapping
    {
        public static string Map(MediaTypesEnum type)
        {
            return type switch
            {
                MediaTypesEnum.Image => "صورة",
                MediaTypesEnum.Video => "فيديو",
                _ => ""
            };
        }
    }
}
