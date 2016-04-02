using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace Tasks
{
    public sealed class TileUpdateTask : IBackgroundTask
    {
        static public void UpdateTile()
        {
            var tile = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareBlock);
            var nlist = tile.GetElementsByTagName("text");
            nlist[0].InnerText = WorkWeek.Current().ToString();
            nlist[1].InnerText = "";

            var notification = new TileNotification(tile);

            // We update our tile every day. So if that doesn't happen, the tile should expire at
            // least a day later.
            notification.ExpirationTime = DateTimeOffset.UtcNow.AddDays(2);

            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            UpdateTile();   
        }
    }
}
