using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Service.Models.User
{
    [Table("user_token")]
    [Serializable]
    public class UserToken
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("refresh_token")]
        public string RefreshToken { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("expired_at")]
        public DateTime ExpiredAt { get; set; }
    }
}
