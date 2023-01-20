namespace BaseProject.BO.Utilities.Helpers;

public static class HttpClientExtensions
{
    public static async Task<T?> ReadBaseContentAs<T>(this HttpResponseMessage response) where T : class
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return null;

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return null;

            var obj = JObject.Parse(dataAsString);

            if (obj is null)
                return null;

            var result = obj.ToObject<T>();

            return result;
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static async Task<List<T>> ReadBaseStructContentAsArray<T>(this HttpResponseMessage response) where T : struct
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return null;

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return null;

            var obj = JArray.Parse(dataAsString);

            if (obj is null)
                return null;

            var result = obj.ToObject<T[]>();

            return result.ToList();
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static async Task<List<T>> ReadBaseContentAsArray<T>(this HttpResponseMessage response) where T : class
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return null;

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return null;

            var obj = JArray.Parse(dataAsString);

            if (obj is null)
                return null;

            var result = obj.ToObject<T[]>();

            return result.ToList();
        }
        catch (Exception)
        {
            return null;
        }
    }
    public static async Task<GenericApiResult<T>> ReadContentAs<T>(this HttpResponseMessage response) where T : class
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return GenericApiResult<T>.GetFailed(response.ReasonPhrase ?? ErrorMessageConstants.API_ERROR);

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return GenericApiResult<T>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE);

            return JsonConvert.DeserializeObject<GenericApiResult<T>>(dataAsString);
        }
        catch (Exception ex)
        {
            return GenericApiResult<T>.GetFailed(ex);
        }
    }
    public static async Task<GenericApiResult<DataSourceResult>> ReadContentAsDataSource<T>(this HttpResponseMessage response, DataSourceRequest request) where T : class
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return GenericApiResult<DataSourceResult>.GetFailed(response.ReasonPhrase ?? ErrorMessageConstants.API_ERROR);

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return GenericApiResult<DataSourceResult>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE);

            var result = JsonConvert.DeserializeObject<DataSourceApiResult<T>>(dataAsString);
            return GenericApiResult<DataSourceResult>.GetSucceed(result.ConvertDataSourceResult());

            //? GenericApiResult<DataSourceResult>.GetSucceed(result.ConvertDataSourceResult())
            //: GenericApiResult<DataSourceResult>.GetFailed(result.Message);
        }
        catch (Exception ex)
        {
            return GenericApiResult<DataSourceResult>.GetFailed(ex);
        }
    }
    public static async Task<GenericApiResult<DataTable>> ReadContentAsDataTable(this HttpResponseMessage response)
    {
        try
        {
            if (!response.IsSuccessStatusCode)
                return GenericApiResult<DataTable>.GetFailed(response.ReasonPhrase ?? ErrorMessageConstants.API_ERROR);

            var dataAsString = await response.Content.ReadAsStringAsync();

            if (dataAsString is null || string.IsNullOrEmpty(dataAsString))
                return GenericApiResult<DataTable>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE);

            var obj = JObject.Parse(dataAsString);

            if (obj is null)
                return GenericApiResult<DataTable>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE);

            var dataobj = obj["data"];
            var messageobj = obj["message"];
            var issuceedobj = obj["isSucceed"];

            var issucceed = issuceedobj?.ToObject<bool>() ?? false;

            if (!issucceed)
                return GenericApiResult<DataTable>.GetFailed(messageobj?.ToObject<string>());

            var data = dataobj.ToObject<DataTableApiResponse>();

            if (data is null)
                return GenericApiResult<DataTable>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE);

            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(data.DataSourceRequest);

            DataTable dataTable = dataSet.Tables[0];

            return dataTable == null ?
                GenericApiResult<DataTable>.GetFailed(ErrorMessageConstants.NULL_API_RESPONSE) :
                GenericApiResult<DataTable>.GetSucceed(dataTable);

        }
        catch (Exception ex)
        {
            return GenericApiResult<DataTable>.GetFailed(ex);
        }
    }

    private static string GetUriWithQueryString(string requestUri,
            Dictionary<string, string> queryStringParams)
    {
        bool startingQuestionMarkAdded = false;
        var sb = new StringBuilder();
        sb.Append(requestUri);
        foreach (var parameter in queryStringParams)
        {
            if (parameter.Value == null)
            {
                continue;
            }

            sb.Append(startingQuestionMarkAdded ? '&' : '?');
            sb.Append(parameter.Key);
            sb.Append('=');
            sb.Append(parameter.Value);
            startingQuestionMarkAdded = true;
        }
        return sb.ToString();
    }
}
