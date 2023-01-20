namespace BaseProject.Domain.Models.ViewModels;

public class DataSourceApiResult<T>
{
    public DataSourceApiResult()
    {

    }
    public DataSourceApiResult(List<T> data, int total)
    {
        Data = data;
        Total = total;
    }

    public List<T> Data { get; set; }

    public int Total { get; set; }

    public IEnumerable<AggregateResult> AggregateResults { get; set; }

    public object Errors { get; set; }


    public DataSourceResult ConvertDataSourceResult() => new DataSourceResult { Data = Data, Total = Total, Errors = Errors, AggregateResults = AggregateResults };
}

public class DataSourceApiRequest : BasePaginationRequest
{
    public DataSourceRequest? Request { get; set; }
    public string? RequestFilters { get; set; }
    public List<string>? Filters { get; set; }
}
public class VehicleDataPaginationRequest : DataSourceApiRequest
{
    public Guid? CustomerId { get; set; }
    public Guid? VehicleId { get; set; }
}
public class RangeeDataPaginationRequest : DataSourceApiRequest
{
    public Guid? CustomerId { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? OrderId { get; set; }
    public DateTime? FilterStart { get; set; }
    public DateTime? FilterEnd { get; set; }
}

public class DataTableApiRequest : DataSourceApiRequest
{
    public List<Guid>? statusfilters { get; set; }
    public List<string>? statustagfilters { get; set; }
    public Guid? CanvasId { get; set; }
    public Guid? PersonaId { get; set; }
}