using System;
using System.Data;
using mojoPortal.Data;

namespace ironclad.Data.Reservations
{
	/// <summary> Class DBReservations. </summary>
	public static class DBReservations
	{
		/// <summary> Inserts a row in the mp_ReservationEvents table. Returns new integer id. </summary>
		/// <param name="itemGuid"> itemGuid </param>
		/// <param name="moduleGuid"> moduleGuid </param>
		/// <param name="moduleId"> moduleID </param>
		/// <param name="title"> title </param>
		/// <param name="description"> description </param>
		/// <param name="imageName"> imageName </param>
		/// <param name="startDate"> The start date. </param>
		/// <param name="endDate"> The end date. </param>
		/// <param name="userId"> The user identifier. </param>
		/// <param name="userGuid"> userGuid </param>
		/// <param name="location"> location </param>
		/// <param name="latitude"> The latitude. </param>
		/// <param name="longitude"> The longitude. </param>
		/// <param name="createdDate"> createdDate </param>
		/// <param name="lastModUserID"> The last mod user identifier. </param>
		/// <param name="lastModUserGuid"> The last mod user unique identifier. </param>
		/// <param name="lastModUtc"> The last mode UTC. </param>
		/// <returns> int </returns>
		public static int AddReservation(
			Guid itemGuid,
			Guid moduleGuid,
			int moduleId,
			string title,
			string description,
			string imageName,
			DateTime startDate,
			DateTime endDate,
			int userId,
			Guid userGuid,
			string location,
			string latitude,
			string longitude,
			DateTime createdDate,
			int lastModUserID,
			Guid lastModUserGuid,
			DateTime lastModUtc)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Reservation_Insert", 17);
			sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
			sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, ParameterDirection.Input, title);
			sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
			sph.DefineSqlParameter("@ImageName", SqlDbType.NVarChar, ParameterDirection.Input, imageName);
			sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
			sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
			sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
			sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
			sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
			sph.DefineSqlParameter("@Location", SqlDbType.NVarChar, -1, ParameterDirection.Input, location);
			sph.DefineSqlParameter("@Latitude", SqlDbType.NVarChar, -1, ParameterDirection.Input, latitude);
			sph.DefineSqlParameter("@Longitude", SqlDbType.NVarChar, -1, ParameterDirection.Input, longitude);
			sph.DefineSqlParameter("@LastModUserID", SqlDbType.Int, -1, ParameterDirection.Input, lastModUserID);
			sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, -1, ParameterDirection.Input, lastModUserGuid);
			sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, -1, ParameterDirection.Input, lastModUtc);

			var newId = Convert.ToInt32(sph.ExecuteScalar());
			return newId;
		}

		/// <summary> Updates a row in the mp_Reservation table. Returns true if row updated. </summary>
		/// <param name="itemId"> The item identifier. </param>
		/// <param name="ItemGuid"> The item unique identifier. </param>
		/// <param name="moduleId"> The module identifier. </param>
		/// <param name="moduleGuid"> The module unique identifier. </param>
		/// <param name="title"> title </param>
		/// <param name="description"> description </param>
		/// <param name="imageName"> imageName </param>
		/// <param name="startDate"> startDate </param>
		/// <param name="endDate"> endDate </param>
		/// <param name="location"> location </param>
		/// <param name="latitude"> The latitude. </param>
		/// <param name="longitude"> The longitude. </param>
		/// <param name="lastModUserId"> The last mod user identifier. </param>
		/// <param name="lastModUserGuid"> lastModUserGuid </param>
		/// <param name="lastModUtc"> The last mod UTC. </param>
		/// <returns> bool </returns>
		public static bool UpdateReservation(
			int itemId,
			Guid ItemGuid,
			int moduleId,
			Guid moduleGuid,
			string title,
			string description,
			string imageName,
			DateTime startDate,
			DateTime endDate,
			string location,
			string latitude,
			string longitude,
			int lastModUserId,
			Guid lastModUserGuid,
			DateTime lastModUtc)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Reservation_Update", 15);
			sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
			sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, ItemGuid);
			sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
			sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
			sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, ParameterDirection.Input, title);
			sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
			sph.DefineSqlParameter("@ImageName", SqlDbType.NVarChar, ParameterDirection.Input, imageName);
			sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
			sph.DefineSqlParameter("@EndDate", SqlDbType.SmallDateTime, ParameterDirection.Input, endDate);
			sph.DefineSqlParameter("@Location", SqlDbType.NVarChar, -1, ParameterDirection.Input, location);
			sph.DefineSqlParameter("@Latitude", SqlDbType.NVarChar, -1, ParameterDirection.Input, latitude);
			sph.DefineSqlParameter("@Longitude", SqlDbType.NVarChar, -1, ParameterDirection.Input, longitude);
			sph.DefineSqlParameter("@LastModUserId", SqlDbType.Int, ParameterDirection.Input, lastModUserId);
			sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModUserGuid);
			sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);

			var rowsAffected = sph.ExecuteNonQuery();
			return rowsAffected > -1;
		}

		/// <summary> Deletes the reservation. </summary>
		/// <param name="itemId"> The item identifier. </param>
		/// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
		public static bool DeleteReservation(int itemId)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Reservation_Delete", 1);
			sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
			var rowsAffected = sph.ExecuteNonQuery();
			return rowsAffected > -1;
		}

		/// <summary> Deletes the by module. </summary>
		/// <param name="moduleId"> The module identifier. </param>
		/// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
		public static bool DeleteByModule(int moduleId)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Reservation_DeleteByModule", 1);
			sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
			var rowsAffected = sph.ExecuteNonQuery();
			return rowsAffected > -1;
		}

		/// <summary> Deletes the by site. </summary>
		/// <param name="siteId"> The site identifier. </param>
		/// <returns> <c>true</c> if XXXX, <c>false</c> otherwise. </returns>
		public static bool DeleteBySite(int siteId)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_Reservation_DeleteBySite", 1);
			sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
			var rowsAffected = sph.ExecuteNonQuery();
			return rowsAffected > -1;
		}

		/// <summary> Gets the reservation. </summary>
		/// <param name="itemId"> The item identifier. </param>
		/// <returns> IDataReader. </returns>
		public static IDataReader GetReservation(int itemId)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Reservation_SelectOne", 1);
			sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
			return sph.ExecuteReader();
		}

		/// <summary> Gets the reservations. </summary>
		/// <param name="moduleId"> The module identifier. </param>
		/// <param name="startDate"> The start date. </param>
		/// <param name="endDate"> The end date. </param>
		/// <returns> DataSet. </returns>
		public static DataSet GetReservations(int moduleId, DateTime startDate, DateTime endDate)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Reservation_SelectByDate", 3);
			sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
			sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
			sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
			return sph.ExecuteDataset();
		}

		/// <summary> Gets the reservation table. </summary>
		/// <param name="moduleId"> The module identifier. </param>
		/// <param name="startDate"> The start date. </param>
		/// <param name="endDate"> The end date. </param>
		/// <returns> DataTable. </returns>
		public static DataTable GetReservationTable(int moduleId, DateTime startDate, DateTime endDate)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_Reservation_SelectByDate", 3);
			sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
			sph.DefineSqlParameter("@BeginDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
			sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);

			var dt = new DataTable();

			dt.Columns.Add("ItemID", typeof(int));
			dt.Columns.Add("ItemGuid", typeof(Guid));
			dt.Columns.Add("ModuleID", typeof(int));
			dt.Columns.Add("ModuleGuid", typeof(Guid));
			dt.Columns.Add("Title", typeof(string));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("ImageName", typeof(string));
			dt.Columns.Add("StartDate", typeof(DateTime));
			dt.Columns.Add("EndDate", typeof(DateTime));
			dt.Columns.Add("Location", typeof(string));
			dt.Columns.Add("Latitude", typeof(string));
			dt.Columns.Add("Longitude", typeof(string));
			dt.Columns.Add("LastModUserID", typeof(int));
			dt.Columns.Add("LastModUserGuid", typeof(Guid));
			dt.Columns.Add("LastModUtc", typeof(string));

			using (var reader = sph.ExecuteReader())
				while (reader.Read())
				{
					var row = dt.NewRow();

					row["ItemID"] = reader["ItemID"];
					row["ItemGuid"] = reader["ItemGuid"];
					row["ModuleID"] = reader["ModuleID"];
					row["ModuleGuid"] = reader["ModuleGuid"];
					row["Title"] = reader["Title"];
					row["Description"] = reader["Description"];
					row["ImageName"] = reader["ImageName"];
					row["StartDate"] = reader["StartDate"];
					row["EndDate"] = reader["EndDate"];
					row["Location"] = reader["Location"];
					row["Latitude"] = reader["Latitude"];
					row["Longitude"] = reader["Longitude"];
					row["LastModUserID"] = reader["LastModUserID"];
					row["LastModUserGuid"] = reader["LastModUserGuid"];
					row["LastModUtc"] = reader["LastModUtc"];

					dt.Rows.Add(row);
				}
			return dt;
		}

		/// <summary> Gets the reservations by page. </summary>
		/// <param name="siteId"> The site identifier. </param>
		/// <param name="pageId"> The page identifier. </param>
		/// <returns> IDataReader. </returns>
		public static IDataReader GetReservationsByPage(int siteId, int pageId)
		{
			var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_ReservationEvents_SelectByPage", 2);
			sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
			sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
			return sph.ExecuteReader();
		}
	}
}
