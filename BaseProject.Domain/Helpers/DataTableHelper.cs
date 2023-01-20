namespace BaseProject.Domain.Helpers;

public class DataTableApiResponse
{
    public string DataSourceRequest { get; set; }
    public int Total { get; set; }
}

public class ListViewApiResponse<T>
{
    public ListViewApiResponse()
    {

    }
    public ListViewApiResponse(List<T> data, int total)
    {
        Data = data;
        Total = total;
    }

    public List<T> Data { get; set; }
    public int Total { get; set; }
}

public class BasePaginationRequest
{
    public int Page
    {
        get
        {
            return _page;
        }
        set => _page = value < 1 ? 1 : value;
    }
    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set => _pageSize = value < 1 ? 1 : value;
    }
    private int _pageSize;
    private int _page;
}



public static class DataTableHelper
{
    public static string ConvertToHtmlTable(DataTable dt)
    {
        StringBuilder html = new StringBuilder();

        if (dt is not null)
        {
            //Table start.
            html.Append("<table class = 'table'>");

            //Building the Header row.
            html.Append("<tr>");
            foreach (DataColumn column in dt.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr>");

            //Building the Data rows.
            foreach (DataRow row in dt.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dt.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }

            //Table end.
            html.Append("</table>");
        }

        return html.ToString();
    }
}
