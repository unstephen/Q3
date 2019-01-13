using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConst {

    public const int cardMaxNum = 3;
    public const int pokerMax = 13;
    public const int pokerCount = 52;
    public const int pageSize = 10;

    public const string httpUrl = "http://47.92.73.235:8080";
    public const string ipadress = "47.92.73.235";
    public const int port = 8080;
    public const int tcp_port = 8081;
    public const string _login = "/login";
    public const string _mainPage = "/user/info";
    public const string _shop = "/goods/list";
    public const string _order = " /goods/pay";
    public const string _myClub = "/club/list";
    public const string _clubInfo = "/club/info";
    public const string _createClub = "/club/create";
    public const string _searchClub = "/club/search";
    public const string _applyClub = "/club/apply";
    public const string _handleRequest = "/apply/deal";
    public const string _history = "/history/overview";
    public const string _searchHistory = "/history/list";
    public const string _searchRoom = "/room/search";
    public const string _joinRoom = "/room/join";
    public const string _createRoom = "/room/create";
    public const float OUT_CARD_SPAN = 0.9f;

    public const string weixinUrl = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code";
}
