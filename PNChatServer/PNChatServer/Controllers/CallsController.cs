using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PNChatServer.Dto;
using PNChatServer.Repository;

namespace PNChatServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallsController : ControllerBase
    {
        private ICallService _callService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CallsController(ICallService callService, IHttpContextAccessor contextAccessor)
        {
            _callService = callService;
            _contextAccessor = contextAccessor;
        }


        [Route("call/{userCode}")]
        [HttpGet]
        public IActionResult Call(string userCode)
        {
            ResponseAPI responeAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                responeAPI.Data = _callService.Call(userSession, userCode);

                return Ok(responeAPI);
            }
            catch (Exception ex)
            {
                responeAPI.Message = ex.Message;
                return BadRequest(responeAPI);
            }
        }

        [Route("get-history")]
        [HttpGet]
        public IActionResult GetHistory()
        {
            ResponseAPI responeAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                responeAPI.Data = _callService.GetCallHistory(userSession);

                return Ok(responeAPI);
            }
            catch (Exception ex)
            {
                responeAPI.Message = ex.Message;
                return BadRequest(responeAPI);
            }
        }

        [Route("get-history/{key}")]
        [HttpGet]
        public IActionResult GetHistoryById(string key)
        {
            ResponseAPI responeAPI = new ResponseAPI();
            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                responeAPI.Data = _callService.GetHistoryById(userSession, key);

                return Ok(responeAPI);
            }
            catch (Exception ex)
            {
                responeAPI.Message = ex.Message;
                return BadRequest(responeAPI);
                
            }
        }


        [Route("join-video-call")]
        [HttpGet]
        public IActionResult JoinVideoCall(string url)
        {
            ResponseAPI responeAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                _callService.JoinVideoCall(userSession, url);

                return Ok(responeAPI);
            }
            catch (Exception ex)
            {
                responeAPI.Message = ex.Message;
                return BadRequest(responeAPI);
            }
        }


        [Route("cancel-video-call")]
        [HttpGet]
        public IActionResult CancelVideoCall(string url)
        {
            ResponseAPI responeAPI = new ResponseAPI();

            try
            {
                string userSession = SystemAuthorization.GetCurrentUser(_contextAccessor);
                _callService.CancelVideoCall(userSession, url);

                return Ok(responeAPI);
            }
            catch (Exception ex)
            {
                responeAPI.Message = ex.Message;
                return BadRequest(responeAPI);
            }
        }
    }
}
