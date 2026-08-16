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

        public static string Map(string type)
        {
            if(type == MediaTypesEnum.Image.ToString())
                return Map(MediaTypesEnum.Image);
            if(type == MediaTypesEnum.Video.ToString())
                return Map(MediaTypesEnum.Video);
            return "";
        }
    }
}
