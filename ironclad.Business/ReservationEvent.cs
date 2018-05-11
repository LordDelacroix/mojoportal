using System;
using System.Data;
using System.Globalization;
using ironclad.Data.Reservations;
using log4net;
using mojoPortal.Business;

namespace ironclad.Business
{
    /// <summary> Represents an event calendar </summary>
    /// <seealso cref="mojoPortal.Business.IIndexableContent" />
    public class ReservationEvent : IIndexableContent
    {
        /// <summary> The feature unique identifier </summary>
        private const string featureGuid = "bfa71b56-bd26-459d-a431-582fd0943317";

        /// <summary> The log </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReservationEvent));

        /// <summary> Initializes a new instance of the <see cref="ReservationEvent"/> class. </summary>
        public ReservationEvent() { }

        /// <summary> Initializes a new instance of the <see cref="ReservationEvent"/> class. </summary>
        /// <param name="itemId"> The item identifier. </param>
        public ReservationEvent(int itemId)
        {
            GetReservationEvent(itemId);
        }

        /// <summary> Gets the feature unique identifier. </summary>
        /// <value> The feature unique identifier. </value>
        public static Guid FeatureGuid => new Guid(featureGuid);

        /// <summary> Gets or sets the item identifier. </summary>
        /// <value> The item identifier. </value>
        public int ItemId { get; set; } = -1;

        /// <summary> Gets the item unique identifier. </summary>
        /// <value> The item unique identifier. </value>
        public Guid ItemGuid { get; private set; } = Guid.Empty;

        /// <summary> Gets or sets the module identifier. </summary>
        /// <value> The module identifier. </value>
        public int ModuleId { get; set; } = -1;

        /// <summary> Gets or sets the module unique identifier. </summary>
        /// <value> The module unique identifier. </value>
        public Guid ModuleGuid { get; set; } = Guid.Empty;

        /// <summary> Gets or sets the title. </summary>
        /// <value> The title. </value>
        public string Title { get; set; } = string.Empty;

        /// <summary> Gets or sets the description. </summary>
        /// <value> The description. </value>
        public string Description { get; set; } = string.Empty;

        /// <summary> Gets or sets the name of the image. </summary>
        /// <value> The name of the image. </value>
        public string ImageName { get; set; } = string.Empty;

        /// <summary> Gets or sets the start date. </summary>
        /// <value> The start date. </value>
        public DateTime StartDate { get; set; } = DateTime.Now;

        /// <summary> Gets or sets the end date. </summary>
        /// <value> The end date. </value>
        public DateTime EndDate { get; set; } = DateTime.Now;

        /// <summary> Gets or sets the created date. </summary>
        /// <value> The created date. </value>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary> Gets or sets the location. </summary>
        /// <value> The location. </value>
        public string Location { get; set; } = string.Empty;

        /// <summary> Gets or sets the latitude. </summary>
        /// <value> The latitude. </value>
        public string Latitude { get; set; } = string.Empty;

        /// <summary> Gets or sets the longitude. </summary>
        /// <value> The longitude. </value>
        public string Longitude { get; set; } = string.Empty;

        /// <summary> Gets or sets the user identifier. </summary>
        /// <value> The user identifier. </value>
        public int UserId { get; set; } = -1;

        /// <summary> Gets or sets the user unique identifier. </summary>
        /// <value> The user unique identifier. </value>
        public Guid UserGuid { get; set; } = Guid.Empty;

        /// <summary> Gets or sets the last mod user unique identifier. </summary>
        /// <value> The last mod user unique identifier. </value>
        public Guid LastModUserGuid { get; set; } = Guid.Empty;

        /// <summary> Gets or sets the last mod user identifier. </summary>
        /// <value> The last mod user identifier. </value>
        public int LastModUserId { get; set; } = -1;

        /// <summary> Gets or sets the last mod UTC. </summary>
        /// <value> The last mod UTC. </value>
        public DateTime LastModUtc { get; set; } = DateTime.UtcNow;

        /// <summary> This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects. </summary>
        /// <value> The site identifier. </value>
        public int SiteId { get; set; } = -1;

        /// <summary> This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects. </summary>
        /// <value> The search index path. </value>
        public string SearchIndexPath { get; set; } = string.Empty;

        /// <summary> Gets the reservation event. </summary>
        /// <param name="itemId"> The item identifier. </param>
        private void GetReservationEvent(int itemId)
        {
            using (var reader = DBReservations.GetReservation(itemId))
                if (reader.Read())
                {
                    ItemId = Convert.ToInt32(reader["ItemID"], CultureInfo.InvariantCulture);
                    ModuleId = Convert.ToInt32(reader["ModuleID"], CultureInfo.InvariantCulture);
                    Title = reader["Title"].ToString();
                    Description = reader["Description"].ToString();
                    ImageName = reader["ImageName"].ToString();
                    StartDate = Convert.ToDateTime(reader["StartDate"], CultureInfo.CurrentCulture);
                    EndDate = Convert.ToDateTime(reader["StartDate"], CultureInfo.CurrentCulture);
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"], CultureInfo.CurrentCulture);
                    UserId = Convert.ToInt32(reader["UserID"], CultureInfo.InvariantCulture);
                    UserGuid = new Guid(reader["UserGuid"].ToString());
                    Location = reader["Location"].ToString();
                    Latitude = reader["Latitude"].ToString();
                    Longitude = reader["Longitude"].ToString();
                    LastModUserId = Convert.ToInt32(reader["LastModUserId"], CultureInfo.InvariantCulture);

                    var u = reader["ItemGuid"].ToString();
                    if (u.Length == 36) ItemGuid = new Guid(u);

                    u = reader["UserGuid"].ToString();
                    if (u.Length == 36) UserGuid = new Guid(u);

                    u = reader["LastModUserGuid"].ToString();
                    if (u.Length == 36) LastModUserGuid = new Guid(u);

                    u = reader["ModuleGuid"].ToString();
                    if (u.Length == 36) ModuleGuid = new Guid(u);

                    if (reader["LastModUtc"] != DBNull.Value)
                        LastModUtc = Convert.ToDateTime(reader["LastModUtc"], CultureInfo.CurrentCulture);
                }
        }

        /// <summary> Creates this instance. </summary>
        /// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
        private bool Create()
        {
            ItemGuid = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;

            var newId = DBReservations.AddReservation(
                ItemGuid,
                ModuleGuid,
                ModuleId,
                Title,
                Description,
                ImageName,
                StartDate,
                EndDate,
                UserId,
                UserGuid,
                Location,
                Latitude,
                Longitude,
                CreatedDate,
                LastModUserId,
                LastModUserGuid,
                LastModUtc);

            ItemId = newId;

            var result = newId > -1;

            if (result)
            {
                var e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;
        }

        /// <summary> Updates this instance. </summary>
        /// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
        private bool Update()
        {
            LastModUtc = DateTime.UtcNow;

            var result = DBReservations.UpdateReservation(
                ItemId,
                ItemGuid,
                ModuleId,
                ModuleGuid,
                Title,
                Description,
                ImageName,
                StartDate,
                EndDate,
                Location,
                Latitude,
                Longitude,
                LastModUserId,
                LastModUserGuid,
                LastModUtc);

            if (result)
                OnContentChanged(new ContentChangedEventArgs());

            return result;
        }

        /// <summary> Saves this instance. </summary>
        /// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
        public bool Save()
        {
            return ItemId > 0
                ? Update()
                : Create();
        }

        /// <summary> Deletes this instance. </summary>
        /// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
        public bool Delete()
        {
            var result = DBReservations.DeleteReservation(ItemId);

            if (result)
                OnContentChanged(new ContentChangedEventArgs { IsDeleted = true });

            return result;
        }

        /// <summary> Deletes the by item identifier. </summary>
        /// <param name="itemID"> The item identifier. </param>
        /// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
        public static bool DeleteByItemID(int itemID)
        {
            return DBReservations.DeleteReservation(itemID);
        }

        /// <summary> Deletes the by module. </summary>
        /// <param name="moduleId"> The module identifier. </param>
        /// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
        public static bool DeleteByModule(int moduleId)
        {
            return DBReservations.DeleteByModule(moduleId);
        }

        /// <summary> Deletes the by site. </summary>
        /// <param name="siteId"> The site identifier. </param>
        /// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
        public static bool DeleteBySite(int siteId)
        {
            return DBReservations.DeleteBySite(siteId);
        }

        /// <summary> Gets the events. </summary>
        /// <param name="moduleId"> The module identifier. </param>
        /// <param name="startDate"> The start date. </param>
        /// <param name="endDate"> The end date. </param>
        /// <returns> DataSet. </returns>
        public static DataSet GetEvents(int moduleId, DateTime startDate, DateTime endDate)
        {
            return DBReservations.GetReservations(moduleId, startDate, endDate);
        }

        /// <summary> Gets the events table. </summary>
        /// <param name="moduleId"> The module identifier. </param>
        /// <param name="startDate"> The start date. </param>
        /// <param name="endDate"> The end date. </param>
        /// <returns> DataTable. </returns>
        public static DataTable GetEventsTable(int moduleId, DateTime startDate, DateTime endDate)
        {
            return DBReservations.GetReservationTable(moduleId, startDate, endDate);
        }

        /// <summary> Gets the events by page. </summary>
        /// <param name="siteId"> The site identifier. </param>
        /// <param name="pageId"> The page identifier. </param>
        /// <returns> DataTable. </returns>
        public static DataTable GetEventsByPage(int siteId, int pageId)
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add("ItemID", typeof(int));
            dataTable.Columns.Add("ItemGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("Title", typeof(string));
            dataTable.Columns.Add("Description", typeof(string));
            dataTable.Columns.Add("ViewRoles", typeof(string));
            dataTable.Columns.Add("CreatedDate", typeof(DateTime));
            dataTable.Columns.Add("LastModUtc", typeof(DateTime));
            dataTable.Columns.Add("Location", typeof(string));
            dataTable.Columns.Add("Latitude", typeof(string));
            dataTable.Columns.Add("Longitude", typeof(string));

            using (var reader = DBReservations.GetReservationsByPage(siteId, pageId))
            {
                while (reader.Read())
                {
                    var row = dataTable.NewRow();

                    row["ItemID"] = reader["ItemID"];
                    row["ItemGuid"] = reader["ItemGuid"];
                    row["ModuleID"] = reader["ModuleID"];
                    row["ModuleGuid"] = reader["ModuleGuid"];
                    row["ModuleTitle"] = reader["ModuleTitle"];
                    row["Title"] = reader["Title"];
                    row["Description"] = reader["Description"];
                    row["ViewRoles"] = reader["ViewRoles"];
                    row["CreatedDate"] = Convert.ToDateTime(reader["CreatedDate"]);
                    row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
                    row["Location"] = reader["Location"];
                    row["Latitude"] = reader["Latitude"];
                    row["Longitude"] = reader["Longitude"];

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        /// <summary> Occurs when [content changed]. </summary>
        public event ContentChangedEventHandler ContentChanged;

        /// <summary> Handles the <see cref="E:ContentChanged" /> event. </summary>
        /// <param name="e"> The <see cref="ContentChangedEventArgs"/> instance containing the event data. </param>
        protected void OnContentChanged(ContentChangedEventArgs e)
        {
            ContentChanged?.Invoke(this, e);
        }
    }
}