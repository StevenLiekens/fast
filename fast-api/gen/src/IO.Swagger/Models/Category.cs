/*
 * Fast Gaming Community API
 *
 * API serving data gathered and analyzed by our lord and savior Cornix
 *
 * OpenAPI spec version: 0.0.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Models
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Category : IEquatable<Category>
    { 
        /// <summary>
        /// Gets or Sets CategoryId
        /// </summary>
        [Required]
        [DataMember(Name="categoryId")]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [Required]
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Info
        /// </summary>
        [Required]
        [DataMember(Name="info")]
        public string Info { get; set; }

        /// <summary>
        /// Gets or Sets CategoryItems
        /// </summary>
        [DataMember(Name="categoryItems")]
        public List<Item> CategoryItems { get; set; }

        /// <summary>
        /// Gets or Sets Buy
        /// </summary>
        [DataMember(Name="buy")]
        public int? Buy { get; set; }

        /// <summary>
        /// Gets or Sets Sell
        /// </summary>
        [DataMember(Name="sell")]
        public int? Sell { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Category {\n");
            sb.Append("  CategoryId: ").Append(CategoryId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Info: ").Append(Info).Append("\n");
            sb.Append("  CategoryItems: ").Append(CategoryItems).Append("\n");
            sb.Append("  Buy: ").Append(Buy).Append("\n");
            sb.Append("  Sell: ").Append(Sell).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Category)obj);
        }

        /// <summary>
        /// Returns true if Category instances are equal
        /// </summary>
        /// <param name="other">Instance of Category to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Category other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    CategoryId == other.CategoryId ||
                    CategoryId != null &&
                    CategoryId.Equals(other.CategoryId)
                ) && 
                (
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) && 
                (
                    Info == other.Info ||
                    Info != null &&
                    Info.Equals(other.Info)
                ) && 
                (
                    CategoryItems == other.CategoryItems ||
                    CategoryItems != null &&
                    CategoryItems.SequenceEqual(other.CategoryItems)
                ) && 
                (
                    Buy == other.Buy ||
                    Buy != null &&
                    Buy.Equals(other.Buy)
                ) && 
                (
                    Sell == other.Sell ||
                    Sell != null &&
                    Sell.Equals(other.Sell)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (CategoryId != null)
                    hashCode = hashCode * 59 + CategoryId.GetHashCode();
                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                    if (Info != null)
                    hashCode = hashCode * 59 + Info.GetHashCode();
                    if (CategoryItems != null)
                    hashCode = hashCode * 59 + CategoryItems.GetHashCode();
                    if (Buy != null)
                    hashCode = hashCode * 59 + Buy.GetHashCode();
                    if (Sell != null)
                    hashCode = hashCode * 59 + Sell.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Category left, Category right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Category left, Category right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
