using System;
using System.Data;
using System.Globalization;
using System.Threading;
using ironclad.Business;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;

namespace ironclad.Features.UI.Reservations.Components
{
	/// <summary> updates the search index when reservationEvent data is changed. </summary>
	/// <seealso cref="IndexBuilderProvider" />
	public class ReservationEventIndexBuilderProvider : IndexBuilderProvider
	{
		/// <summary> The log </summary>
		private static readonly ILog Log = LogManager.GetLogger(typeof(ReservationEventIndexBuilderProvider));
		/// <summary> The reservation feature unique identifier </summary>
		private static readonly Guid reservationFeatureGuid = new Guid("949e081e-ea4e-4fbe-9986-25e41ff67221");

		/// <summary> Rebuilds the index. </summary>
		/// <param name="pageSettings"> The page settings. </param>
		/// <param name="indexPath"> The index path. </param>
		public override void RebuildIndex(PageSettings pageSettings, string indexPath)
		{
			if (WebConfigSettings.DisableSearchIndex) { return; }
			if (pageSettings == null) return;

			//don't index pending/unpublished pages
			if (pageSettings.IsPending) { return; }

			Log.Info("ReservationEventIndexBuilderProvider indexing page - " + pageSettings.PageName);

			try
			{
				var reservationFeature = new ModuleDefinition(reservationFeatureGuid);
				var pageModules = PageModule.GetPageModulesByPage(pageSettings.PageId);
				var dataTable = ReservationEvent.GetEventsByPage(pageSettings.SiteId, pageSettings.PageId);

				foreach (DataRow row in dataTable.Rows)
				{
					var indexItem = new IndexItem
					{
						SiteId = pageSettings.SiteId,
						PageId = pageSettings.PageId,
						PageName = pageSettings.PageName,
						PageIndex = pageSettings.PageIndex,
						ViewRoles = pageSettings.AuthorizedRoles,
						ModuleViewRoles = row["ViewRoles"].ToString(),
						FeatureId = reservationFeatureGuid.ToString(),
						FeatureName = reservationFeature.FeatureName,
						FeatureResourceFile = reservationFeature.ResourceFile,
						ItemId = Convert.ToInt32(row["ItemID"], CultureInfo.InvariantCulture),
						ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture),
						ModuleTitle = row["ModuleTitle"].ToString(),
						Title = row["Title"].ToString(),
						Content = row["Description"].ToString(),
						ViewPage = "Reservations/Details.aspx",
						CreatedUtc = Convert.ToDateTime(row["CreatedDate"]),
						LastModUtc = Convert.ToDateTime(row["LastModUtc"])
					};

					// lookup publish dates
					foreach (var pageModule in pageModules)
					{
						if (indexItem.ModuleId == pageModule.ModuleId)
						{
							indexItem.PublishBeginDate = pageModule.PublishBeginDate;
							indexItem.PublishEndDate = pageModule.PublishEndDate;
						}
					}

					IndexHelper.RebuildIndex(indexItem, indexPath);

					if (Log.IsDebugEnabled) Log.Debug("Indexed " + indexItem.Title);
				}
			}
			catch (System.Data.Common.DbException ex)
			{
				Log.Error(ex);
			}
		}

		/// <summary> Contents the changed handler. </summary>
		/// <param name="sender"> The sender. </param>
		/// <param name="e"> The <see cref="ContentChangedEventArgs"/> instance containing the event data. </param>
		public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
		{
			if (WebConfigSettings.DisableSearchIndex || !(sender is ReservationEvent)) return;

			var reservationEvent = (ReservationEvent)sender;
			var siteSettings = CacheHelper.GetCurrentSiteSettings();
			reservationEvent.SiteId = siteSettings.SiteId;
			reservationEvent.SearchIndexPath = IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

			if (e.IsDeleted)
			{
				// get list of pages where this module is published
				var pageModules = PageModule.GetPageModulesByModule(reservationEvent.ModuleId);

				foreach (var pageModule in pageModules)
					IndexHelper.RemoveIndexItem(pageModule.PageId, reservationEvent.ModuleId, reservationEvent.ItemId);
			}
			else
			{
				if (ThreadPool.QueueUserWorkItem(IndexItem, reservationEvent))
				{
					if (Log.IsDebugEnabled) Log.Debug("ReservationEventIndexBuilderProvider.IndexItem queued");
				}
				else
				{
					Log.Error("Failed to queue a thread for ReservationEventIndexBuilderProvider.IndexItem");
				}
			}
		}

		/// <summary> Indexes the item. </summary>
		/// <param name="o"> The o. </param>
		private static void IndexItem(object o)
		{
			if (WebConfigSettings.DisableSearchIndex || !(o is ReservationEvent)) return;

			IndexItem((ReservationEvent)o);
		}

		/// <summary> Indexes the item. </summary>
		/// <param name="reservationEvent"> The reservation event. </param>
		private static void IndexItem(ReservationEvent reservationEvent)
		{
			if (WebConfigSettings.DisableSearchIndex || reservationEvent == null) return;

			try
			{
				var module = new Module(reservationEvent.ModuleId);
				var reservationFeature = new ModuleDefinition(reservationFeatureGuid);

				// get list of pages where this module is published
				var pageModules = PageModule.GetPageModulesByModule(reservationEvent.ModuleId);

				foreach (var pageModule in pageModules)
				{
					var pageSettings = new PageSettings(reservationEvent.SiteId, pageModule.PageId);

					//don't index pending/unpublished pages
					if (pageSettings.IsPending) { continue; }

					var indexItem = new IndexItem
					{
						SiteId = reservationEvent.SiteId,
						PageId = pageSettings.PageId,
						PageName = pageSettings.PageName,
						ViewRoles = pageSettings.AuthorizedRoles,
						ModuleViewRoles = module.ViewRoles,
						ItemId = reservationEvent.ItemId,
						ModuleId = reservationEvent.ModuleId,
						ViewPage = "Reservation/Details.aspx",
						FeatureId = reservationFeatureGuid.ToString(),
						FeatureName = reservationFeature.FeatureName,
						FeatureResourceFile = reservationFeature.ResourceFile,
						ModuleTitle = module.ModuleTitle,
						Title = reservationEvent.Title,
						Content = reservationEvent.Description,
						PublishBeginDate = pageModule.PublishBeginDate,
						PublishEndDate = pageModule.PublishEndDate,
						CreatedUtc = reservationEvent.CreatedDate,
						LastModUtc = reservationEvent.LastModUtc
					};

					if (reservationEvent.SearchIndexPath.Length > 0)
						indexItem.IndexPath = reservationEvent.SearchIndexPath;

					IndexHelper.RebuildIndex(indexItem);
				}

				if (Log.IsDebugEnabled) Log.Debug("Indexed " + reservationEvent.Title);
			}
			catch (System.Data.Common.DbException ex)
			{
				Log.Error("ReservationEventIndexBuilderProvider.IndexItem", ex);
			}
		}
	}
}
