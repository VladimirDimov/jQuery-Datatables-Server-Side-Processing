namespace JQDT
{
    using JQDT.DataProcessing;
    using JQDT.ModelBinders;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false)]
    public class JQDataTableAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            try
            {
                this.Execute(filterContext);
            }
            catch (Exception ex)
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.Result = this.FormatResult(new
                {
                    error = ex.Message
                });
            }
        }

        private void Execute(ActionExecutedContext filterContext)
        {
            IQueryable<object> data = (IQueryable<object>)filterContext.Controller.ViewData.Model;

            var modelBinder = new FormModelBinder();
            var requestModel = modelBinder.BindModel(filterContext);

            var dataProcessChain = this.GetDataProcessChain();
            var processedData = dataProcessChain.ProcessData(data, requestModel);

            filterContext.Result = this.FormatResult(new
            {
                draw = requestModel.TableParameters.draw,
                recordsTotal = data.Count(),
                recordsFiltered = this.GetRecordsFiltered(dataProcessChain),
                data = processedData.ToList()
            });

            base.OnActionExecuted(filterContext);
        }

        private object GetRecordsFiltered(IDataProcess dataProcessChain)
        {
            return
                ((IDataProcessChain)dataProcessChain)
                    .DataProcessors
                    .First(p => p.GetType() == typeof(CustomFiltersDataProcessor))
                    .ProcessedData.Count();
        }

        private IDataProcess GetDataProcessChain()
        {
            var dataProcessChain = new DataProcessChain();
            dataProcessChain.AddDataProcessor(new FilterDataProcessor());
            dataProcessChain.AddDataProcessor(new CustomFiltersDataProcessor());
            dataProcessChain.AddDataProcessor(new OrderDataProcessor());
            dataProcessChain.AddDataProcessor(new PagingDataProcessor());

            return dataProcessChain;
        }

        private ActionResult FormatResult(object resultModel)
        {
            var jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jsonResult.Data = resultModel;

            return jsonResult;
        }
    }
}