var 
http = require('http'),
url  = require('url'),
path = require('path'),
fs   = require('fs'),
os   = require('os');

function getIPv4(){
   var interfaces = os.networkInterfaces();//获取网络接口列表
   var ipv4s = [];//同一接口可能有不止一个IP4v地址，所以用数组存

    Object.keys(interfaces).forEach(function (key){
        interfaces[key].forEach(function (item){

            //跳过IPv6 和 '127.0.0.1'
            if ( 'IPv4' !== item.family || item.internal !== false )return;

            ipv4s.push(item.address);//可用的ipv4s加入数组
            console.log(key+'--'+item.address);
        })        
    })

	return ipv4s[0];//返回一个可用的即可
}

var mime = {
   "html": "text/html",
   "htm": "text/html",
   "css": "text/css",
   "js": "text/javascript",
   "xml": "text/xml",
   "json": "application/json",
   "jpg": "image/jpeg",
   "jpeg": "image/jpeg",
   "png": "image/png",
   "gif": "image/gif",
   "bmp": "image/bmp",
   "svg": "image/svg+xml",
   "ico": "image/x-icon",
   "mp3": "audio/mpeg",
   "wav": "audio/x-wav",
   "mp4": "video/mp4",
   "swf": "application/x-shockwave-flash",
   "woff": "application/x-font-woff"

}


var server = http.createServer(function (req,res){

    var pathname = url.parse(req.url).pathname;;
	var filename = 'D:/WebStorm_WorkPace/potted_plant'+pathname;
    var extname = path.extname(filename);


	 //扩展名含点号如'.html',截掉
     extname = extname ? extname.slice(1) : 'unknown';
    //映射表中查找请求的资源的MIME类型并返回，没有映射均返回'text/plain'类型
    var resContentType = mime[extname] || 'text/plain';

    fs.exists(filename,function (exists){
        if (!exists){
            //文件不存在返回404
            res.writeHead(404,{'Content-Type':'text/plain'});
            res.write('404 Not Found');
            res.end();
        }else {
            //文件存在读取并返回
            fs.readFile(filename,function (err,data){
                if (err){
                    res.writeHead(500,{'Content-Type':'text/plain'});
                    res.end(err);
                }else{
                    res.writeHead(200,{'Content-Type':resContentType});
                    res.write(data);
                    res.end();
                }
            })
        }
    })

});

server.listen('8080',function (){
    console.log('server start on: '+getIPv4()+':8080');
})
