using TinyORM.Core;

namespace TinyLog.DAL.Sqlite;

[Entity]
[Table("", "log_items")]
public class LogItemEntity
{
  [Field("log_itemsid")] public long LogItemsId { get; set; }

  [Field("parentid")] public long? Parent { get; set; }

  [Field("item_typeid")] public long ItemTypesId { get; set; }

  [Field("message")] public string Message { get; set; }

  [Field("moment")] public string SMoment { get; set; }

  public DateTime Moment => DateTime.Parse(SMoment);

  [Field("modify_time")] public string SModifyTime { get; set; }

  public DateTime ModifyTime => DateTime.Parse(SModifyTime);

  [Field("requestid")] public string RequestId { get; set; }

  [Field("writer")] public string Writer { get; set; }

  [Field("application")] public string Application { get; set; }

  [Field("stack_trace")] public string StackTrace { get; set; }
}