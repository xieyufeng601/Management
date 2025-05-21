using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using SqlSugar;

namespace Wen.Models.Entity;

public interface IEntityBaseTreeId
{
    /// <summary>  
    /// 雪花Id  
    /// </summary>  
    [Key]
    public long Id { get; set; }

    /// <summary>  
    /// 雪花Id  
    /// </summary>  
    [DisplayName("父层ID")]
    public string  ParentId { get; set; }

   
    public List<IEntityBaseTreeId> Childs { get; set; }
}
