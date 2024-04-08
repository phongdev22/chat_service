using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using chat_service.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace chat_service.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private HttpClient _httpClient;
        private IConfiguration _config;
        private string? access_token;

        public ConversationController(IConfiguration config)
        {
            _httpClient = new HttpClient();
            _config = config;
            access_token = ""; // Request.Headers["access_token"].ToString() ?? "";
		}

        [HttpGet("listchat")]
        public async Task<IActionResult> ListRecentChat([FromQuery]string? data)
        {

			List<string> lists = new List<string>() {
				"{\"src\": 1,\"time\": 1619401853770,\"type\": \"text\",\"message\": \"Chào shop\",\"message_id\": \"92e5d851aa8178dd2192\",\"from_id\": \"3700646744485476903\",\"to_id\": \"3120036654733951760\",\"from_display_name\": \"Khoa Pham\",\"from_avatar\": \"https://s240-ava-talk.zadn.vn/3/f/4/7/16/240/10aaabe185bc828af44c50ca29ec5d79.jpg\",\"to_display_name\": \"OA\",\"to_avatar\": \"https://s240-ava-talk.zadn.vn/7/c/b/1/1/240/3318c818b07fb3e228eaf8b448c53d3e.jpg\"}",
				"{\"src\": 1,\"time\": 1619401853770,\"type\": \"text\",\"message\": \"Chào shop\",\"message_id\": \"92e5d851aa8178dd2192\",\"from_id\": \"3700646744485476903\",\"to_id\": \"3120036654733951760\",\"from_display_name\": \"Khoa Pham\",\"from_avatar\": \"https://s240-ava-talk.zadn.vn/3/f/4/7/16/240/10aaabe185bc828af44c50ca29ec5d79.jpg\",\"to_display_name\": \"OA\",\"to_avatar\": \"https://s240-ava-talk.zadn.vn/7/c/b/1/1/240/3318c818b07fb3e228eaf8b448c53d3e.jpg\"}",
				"{\"src\": 1,\"time\": 1619401853770,\"type\": \"text\",\"message\": \"Chào shop\",\"message_id\": \"92e5d851aa8178dd2192\",\"from_id\": \"3700646744485476903\",\"to_id\": \"3120036654733951760\",\"from_display_name\": \"Khoa Pham\",\"from_avatar\": \"https://s240-ava-talk.zadn.vn/3/f/4/7/16/240/10aaabe185bc828af44c50ca29ec5d79.jpg\",\"to_display_name\": \"OA\",\"to_avatar\": \"https://s240-ava-talk.zadn.vn/7/c/b/1/1/240/3318c818b07fb3e228eaf8b448c53d3e.jpg\"}",
				"{\"src\": 1,\"time\": 1619401853770,\"type\": \"text\",\"message\": \"Chào shop\",\"message_id\": \"92e5d851aa8178dd2192\",\"from_id\": \"3700646744485476903\",\"to_id\": \"3120036654733951760\",\"from_display_name\": \"Khoa Pham\",\"from_avatar\": \"https://s240-ava-talk.zadn.vn/3/f/4/7/16/240/10aaabe185bc828af44c50ca29ec5d79.jpg\",\"to_display_name\": \"OA\",\"to_avatar\": \"https://s240-ava-talk.zadn.vn/7/c/b/1/1/240/3318c818b07fb3e228eaf8b448c53d3e.jpg\"}",
				"{\"src\": 1,\"time\": 1619401853770,\"type\": \"text\",\"message\": \"Chào shop\",\"message_id\": \"92e5d851aa8178dd2192\",\"from_id\": \"3700646744485476903\",\"to_id\": \"3120036654733951760\",\"from_display_name\": \"Khoa Pham\",\"from_avatar\": \"https://s240-ava-talk.zadn.vn/3/f/4/7/16/240/10aaabe185bc828af44c50ca29ec5d79.jpg\",\"to_display_name\": \"OA\",\"to_avatar\": \"https://s240-ava-talk.zadn.vn/7/c/b/1/1/240/3318c818b07fb3e228eaf8b448c53d3e.jpg\"}",
				"{\"src\": 1,\"time\": 1619401853770,\"type\": \"text\",\"message\": \"Chào shop\",\"message_id\": \"92e5d851aa8178dd2192\",\"from_id\": \"3700646744485476903\",\"to_id\": \"3120036654733951760\",\"from_display_name\": \"Khoa Pham\",\"from_avatar\": \"https://s240-ava-talk.zadn.vn/3/f/4/7/16/240/10aaabe185bc828af44c50ca29ec5d79.jpg\",\"to_display_name\": \"OA\",\"to_avatar\": \"https://s240-ava-talk.zadn.vn/7/c/b/1/1/240/3318c818b07fb3e228eaf8b448c53d3e.jpg\"}",
				"{\"src\": 1,\"time\": 1619401853770,\"type\": \"text\",\"message\": \"Chào shop\",\"message_id\": \"92e5d851aa8178dd2192\",\"from_id\": \"3700646744485476903\",\"to_id\": \"3120036654733951760\",\"from_display_name\": \"Khoa Pham\",\"from_avatar\": \"https://s240-ava-talk.zadn.vn/3/f/4/7/16/240/10aaabe185bc828af44c50ca29ec5d79.jpg\",\"to_display_name\": \"OA\",\"to_avatar\": \"https://s240-ava-talk.zadn.vn/7/c/b/1/1/240/3318c818b07fb3e228eaf8b448c53d3e.jpg\"}",
				"{\"src\": 1,\"time\": 1619401853770,\"type\": \"text\",\"message\": \"Chào shop\",\"message_id\": \"92e5d851aa8178dd2192\",\"from_id\": \"3700646744485476903\",\"to_id\": \"3120036654733951760\",\"from_display_name\": \"Khoa Pham\",\"from_avatar\": \"https://s240-ava-talk.zadn.vn/3/f/4/7/16/240/10aaabe185bc828af44c50ca29ec5d79.jpg\",\"to_display_name\": \"OA\",\"to_avatar\": \"https://s240-ava-talk.zadn.vn/7/c/b/1/1/240/3318c818b07fb3e228eaf8b448c53d3e.jpg\"}",
			};

			return Ok(new { code = 0, chats = lists });

			if (data != null && !Validator.IsValidJson(data)) return Ok(new {Code = 0, Message = "Data is not json format!" });
            if (access_token.IsNullOrEmpty()) return Ok(new { Code = 0, Message = "Access token is invalid"});
            if (data == null) { data = "{\"offset\":0,\"count\":10}";}

			var api_url = _config.GetSection("Zalo")["API_V2"] + "/listrecentchat?data=" + data;
			var request = new HttpRequestMessage(HttpMethod.Get, api_url);

			// Add access token to the headers
			request.Headers.Add("access_token", access_token);
            var response = await _httpClient.GetAsync(api_url);

            return Ok(response);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> DetailConversation(string id, [FromQuery]string? data)
        {

            List<string> chatDetails = new List<string>() { 
                "{\"src\": 1, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"Chào shop\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
                "{\"src\": 1, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"Chào shop\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
				"{\"src\": 0, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"OA đến người dùng\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
                "{\"src\": 1, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"Chào shop\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
				"{\"src\": 0, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"OA đến người dùng\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
                "{\"src\": 1, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"Chào shop\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
                "{\"src\": 1, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"Chào shop\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
				"{\"src\": 0, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"OA đến người dùng\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
				"{\"src\": 0, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"OA đến người dùng\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
                "{\"src\": 1, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"Chào shop\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
                "{\"src\": 1, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"Chào shop\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}",
                "{\"src\": 1, \"time\": 1619401853770, \"type\": \"text\", \"message\": \"Chào shop\", \"message_id\": \"92e5d851aa8178dd2192\", \"from_id\": \"2512523625412515\", \"to_id\": \"3120036654733951760\", \"from_display_name\": \"Khoa Pham\", \"from_avatar\": \"https://s240-ava-talk.zadn.vn/e185bc8ca29ec5d79.jpg\",\"to_display_name\": \"OA\", \"to_avatar\": \"https://s240-ava-talk.zadn.vn/b07fb3e228e448c53d3e.jpg\"}"
            };

            return Ok(new {code = 0, messages = chatDetails});
            
            if (data != null && !Validator.IsValidJson(data)) return Ok(new { Code = 0, Message = "Data is not json format" });
			if (access_token.IsNullOrEmpty()) return Ok(new { Code = 0, Message = "Access token is invalid" });
            
            if (data == null)
            {
                data = JsonConvert.SerializeObject(new
                {
                    offset = 0,
                    count = 10,
                    user_id = id
                }).ToString();
            }

            var api_url = _config.GetSection("Zalo")["API_V2"] + "/conversation?data=" + data;
			var request = new HttpRequestMessage(HttpMethod.Get, api_url);
			request.Headers.Add("access_token", access_token);

			var response = await _httpClient.GetAsync(api_url);
            
            return Ok(response);
        }
    }
}
