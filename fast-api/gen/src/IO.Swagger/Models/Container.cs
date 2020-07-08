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
    public partial class Container : IEquatable<Container>
    { 
        /// <summary>
        /// Gets or Sets ContainerId
        /// </summary>
        [Required]
        [DataMember(Name="containerId")]
        public int? ContainerId { get; set; }

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
        /// Gets or Sets Tag
        /// </summary>
        [Required]
        [DataMember(Name="tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Gets or Sets ContainerItems
        /// </summary>
        [DataMember(Name="containerItems")]
        public List<ContainerItem> ContainerItems { get; set; }

        /// <summary>
        /// Gets or Sets ContainerContainers
        /// </summary>
        [DataMember(Name="containerContainers")]
        public List<ContainerContainer> ContainerContainers { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Container {\n");
            sb.Append("  ContainerId: ").Append(ContainerId).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Info: ").Append(Info).Append("\n");
            sb.Append("  Tag: ").Append(Tag).Append("\n");
            sb.Append("  ContainerItems: ").Append(ContainerItems).Append("\n");
            sb.Append("  ContainerContainers: ").Append(ContainerContainers).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Container)obj);
        }

        /// <summary>
        /// Returns true if Container instances are equal
        /// </summary>
        /// <param name="other">Instance of Container to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Container other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    ContainerId == other.ContainerId ||
                    ContainerId != null &&
                    ContainerId.Equals(other.ContainerId)
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
                    Tag == other.Tag ||
                    Tag != null &&
                    Tag.Equals(other.Tag)
                ) && 
                (
                    ContainerItems == other.ContainerItems ||
                    ContainerItems != null &&
                    ContainerItems.SequenceEqual(other.ContainerItems)
                ) && 
                (
                    ContainerContainers == other.ContainerContainers ||
                    ContainerContainers != null &&
                    ContainerContainers.SequenceEqual(other.ContainerContainers)
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
                    if (ContainerId != null)
                    hashCode = hashCode * 59 + ContainerId.GetHashCode();
                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                    if (Info != null)
                    hashCode = hashCode * 59 + Info.GetHashCode();
                    if (Tag != null)
                    hashCode = hashCode * 59 + Tag.GetHashCode();
                    if (ContainerItems != null)
                    hashCode = hashCode * 59 + ContainerItems.GetHashCode();
                    if (ContainerContainers != null)
                    hashCode = hashCode * 59 + ContainerContainers.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Container left, Container right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Container left, Container right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
