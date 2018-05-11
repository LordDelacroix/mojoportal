using System;
using Newtonsoft.Json;

namespace ironclad.Features.UI.Reservations.Components
{
	/// <summary> Class FullCalendarEvent. </summary>
	[Serializable]
	[JsonObject(MemberSerialization.OptIn)]
	public class FullCalendarEvent
	{
		/// <summary> Gets or sets the identifier. </summary>
		/// <value> The identifier. </value>
		[JsonProperty]
		public int id { get; set; }

		/// <summary> Gets or sets the item unique identifier. </summary>
		/// <value> The item unique identifier. </value>
		[JsonProperty]
		public string itemGuid { get; set; }

		/// <summary> Gets or sets the title. </summary>
		/// <value> The title. </value>
		[JsonProperty]
		public string title { get; set; }

		/// <summary> Gets or sets a value indicating whether [all day]. </summary>
		/// <value> <c>true</c> if [all day]; otherwise, <c>false</c>. </value>
		[JsonProperty]
		public bool allDay { get; set; }

		/// <summary> Gets or sets the start. </summary>
		/// <value> The start. </value>
		[JsonProperty]
		public string start { get; set; }

		/// <summary> Gets or sets the end. </summary>
		/// <value> The end. </value>
		[JsonProperty]
		public string end { get; set; }

		/// <summary> Gets or sets the URL. </summary>
		/// <value> The URL. </value>
		[JsonProperty]
		public string url { get; set; }

		/// <summary> Gets or sets the name of the class. </summary>
		/// <value> The name of the class. </value>
		[JsonProperty]
		public string className { get; set; }

		/// <summary> Gets or sets a value indicating whether this <see cref="FullCalendarEvent" /> is editable. </summary>
		/// <value> <c>true</c> if editable; otherwise, <c>false</c>. </value>
		[JsonProperty]
		public bool editable { get; set; }

		/// <summary> Gets or sets a value indicating whether [start editable]. </summary>
		/// <value> <c>true</c> if [start editable]; otherwise, <c>false</c>. </value>
		[JsonProperty]
		public bool startEditable { get; set; }

		/// <summary> Gets or sets a value indicating whether [duration editable]. </summary>
		/// <value> <c>true</c> if [duration editable]; otherwise, <c>false</c>. </value>
		[JsonProperty]
		public bool durationEditable { get; set; }

		/// <summary> Gets or sets a value indicating whether [resource editable]. </summary>
		/// <value> <c>true</c> if [resource editable]; otherwise, <c>false</c>. </value>
		[JsonProperty]
		public bool resourceEditable { get; set; }

		/// <summary> Gets or sets the rendering. </summary>
		/// <value> The rendering. </value>
		[JsonProperty]
		public string rendering { get; set; }

		/// <summary> Gets or sets a value indicating whether this <see cref="FullCalendarEvent" /> is overlap. </summary>
		/// <value> <c>true</c> if overlap; otherwise, <c>false</c>. </value>
		[JsonProperty]
		public bool overlap { get; set; }

		/// <summary> Gets or sets a value indicating whether this <see cref="FullCalendarEvent" /> is constraint. </summary>
		/// <value> <c>true</c> if constraint; otherwise, <c>false</c>. </value>
		[JsonProperty]
		public bool constraint { get; set; }

		/// <summary> Gets or sets the color. </summary>
		/// <value> The color. </value>
		[JsonProperty]
		public string color { get; set; }

		/// <summary> Gets or sets the color of the background. </summary>
		/// <value> The color of the background. </value>
		[JsonProperty]
		public string backgroundColor { get; set; }

		/// <summary> Gets or sets the color of the border. </summary>
		/// <value> The color of the border. </value>
		[JsonProperty]
		public string borderColor { get; set; }

		/// <summary> Gets or sets the color of the text. </summary>
		/// <value> The color of the text. </value>
		[JsonProperty]
		public string textColor { get; set; }

		/// <summary> Gets or sets the created date. </summary>
		/// <value> The created date. </value>
		[JsonProperty]
		public string createdDate { get; set; }

		/// <summary> Gets or sets the user identifier. </summary>
		/// <value> The user identifier. </value>
		[JsonProperty]
		public int userId { get; set; }

		/// <summary> Gets or sets the user unique identifier. </summary>
		/// <value> The user unique identifier. </value>
		[JsonProperty]
		public string userGuid { get; set; }

		/// <summary> Gets or sets the location. </summary>
		/// <value> The location. </value>
		[JsonProperty]
		public string location { get; set; }

		/// <summary> Gets or sets the latitude. </summary>
		/// <value> The latitude. </value>
		[JsonProperty]
		public string latitude { get; set; }

		/// <summary> Gets or sets the longitude. </summary>
		/// <value> The longitude. </value>
		[JsonProperty]
		public string longitude { get; set; }

		/// <summary> Gets or sets the last mod user identifier. </summary>
		/// <value> The last mod user identifier. </value>
		[JsonProperty]
		public int? lastModUserId { get; set; }

		/// <summary> Gets or sets the last mod user unique identifier. </summary>
		/// <value> The last mod user unique identifier. </value>
		[JsonProperty]
		public string lastModUserGuid { get; set; }

		/// <summary> Gets or sets the name of the image. </summary>
		/// <value> The name of the image. </value>
		[JsonProperty]
		public string imageName { get; set; }

		/// <summary> Gets or sets the description. </summary>
		/// <value> The description. </value>
		[JsonProperty]
		public string description { get; set; }

		/// <summary> Gets or sets the module identifier. </summary>
		/// <value> The module identifier. </value>
		[JsonProperty]
		public int moduleId { get; set; }

		/// <summary> Gets or sets the module unique identifier. </summary>
		/// <value> The module unique identifier. </value>
		[JsonProperty]
		public string moduleGuid { get; set; }
	}
}
