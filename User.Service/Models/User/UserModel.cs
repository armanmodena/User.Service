using System;
using Dapper;

namespace User.Service.Models.User
{
    [Table("users")]
    [Serializable]

    public class UserModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        #nullable enable
        public string? Password { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        #nullable enable
        public DateTime? UpdatedAt { get; set; }

        [Column("image_name")]
        #nullable enable
        public string? ImageName { get; set; }

    }
}
