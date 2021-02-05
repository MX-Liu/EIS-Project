
var data_buff = [];
var express = require('express');
var path = require("path");
var fs = require("fs");
var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var port = process.env.PORT || 8080;


var url = require('url');
var zlib = require("zlib");
var querystring = require('querystring');

var dataFIFO = [];
var ID_NUM = [];
var Recieved_data = [];


var mime = {

  "css": "text/css",

  "gif": "image/gif",

  "html": "text/html",

  "ico": "image/x-icon",

  "jpeg": "image/jpeg",

  "jpg": "image/jpeg",

  "js": "text/javascript",

  "json": "application/json",

  "pdf": "application/pdf",

  "png": "image/png",

  "svg": "image/svg+xml",

  "swf": "application/x-shockwave-flash",

  "tiff": "image/tiff",

  "txt": "text/plain",

  "wav": "audio/x-wav",

  "wma": "audio/x-ms-wma",

  "wmv": "video/x-ms-wmv",

  "xml": "text/xml"

};

var config = {
    Expires : {

        fileMatch: /^(gif|png|jpg|js|css)$/ig,

        maxAge: 60*60*24*365
        
    },
    Compress : {
        match: /css|js|html/ig
    }

};


app.use('/',express.static(path.join(__dirname,'/static')));

app.get('/', function(req, res){
  res.sendFile(__dirname + '/FDA.html');
});


io.on('connection', function(socket){
  socket.on('result', function(msg){
	
	data_buff = msg.split(":");
	//ID_NUM = msg.split(",");
	
	for (i = 0; i < 8; i++) 
	{ 
		dataFIFO.push(data_buff[i]);
		Recieved_data = data_buff[i].split(",");
	if  (Recieved_data.length == 1)
	{
			ID_NUM = Recieved_data[0];
			fs.mkdir("./"+ID_NUM+"/",function(err){
		    if (err){
		    }
			else
			{
					//创建以ID为名的文件夹
					console.log(ID_NUM + "目录创建成功。");
					fs.mkdir("./static/"+ID_NUM+"/"+"FDA",function(err){
					if (err){			
					}
					else
					{
						//创建以FDA为名的子文件夹
						console.log(ID_NUM + "FDA目录创建成功。");	
						//创建Single  Measurement 的txt文档
						fs.writeFile("./static/"+ID_NUM+"/"+"FDA/Single measurement.txt", "Fre"+"\t"+"Mag"+"\t"+"Pha",  function(err) {
						   if (err) {
							   return console.error(err);
						   }					  
						});		
						//创建multiple measurement 的txt文档
						fs.writeFile("./static/"+ID_NUM+"/"+"FDA/Multiple measurement.txt", "Fre"+"\t"+"Mag"+"\t"+"Pha"+"\t"+"Times",  function(err) {
						   if (err) {
							   return console.error(err);
						   }					  
						});
					}
					});
					
					//创建以TD为名的子文件夹
					fs.mkdir("./static/"+ID_NUM+"/"+"TD",function(err){
					if (err){			
					}
					else
					{
						console.log(ID_NUM + "TD目录创建成功。");	
						//创建Single  Measurement 的txt文档
						fs.writeFile("./static/"+ID_NUM+"/"+"TD/Single measurement.txt", "Time" + "\t" + "Impedance",  function(err) {
						   if (err) {
							   return console.error(err);
						   }					  
						});		
					}
					});
					
					//创建以DC为名的文件夹
					fs.mkdir("./static/"+ID_NUM+"/"+"DC",function(err){
					if (err){			
					}
					else
					{
						console.log(ID_NUM + "DC目录创建成功。");		
						//创建U_I_R_Data的txt文档
						fs.writeFile("./static/"+ID_NUM+"/"+"DC/U_I_R_Data.txt", "Time" + "\t" + "Voltage" + "\t" + "Current" + "\t" + "Resistance",  function(err) {
						   if (err) {
							   return console.error(err);
						   }					  
						});		
					}
					});
					
					//创建Combination的txt文档
					fs.mkdir("./static/"+ID_NUM+"/"+"Combination",function(err){
					if (err){			
					}
					else
					{
						console.log(ID_NUM + "Combination目录创建成功。");	
						//创建U_I_R_Data的txt文档
						fs.writeFile("./static/"+ID_NUM+"/"+"Combination/Combination_Measurement.txt", "Fre"+"\t"+"Mag"+"\t"+"Pha"+"\t"+"Times",  function(err) {
						   if (err) {
							   return console.error(err);
						   }					  
						});		
					}
					});
			}
			});
		
		
		
		
		
		console.log('ID Nummber :' + ID_NUM);
		
	}
	//FDA数据接收
	else if(Recieved_data.length == 4)
	{
		console.log('receive data :' + data_buff[i]);
		if (Recieved_data[3] == 0)
		{
			fs.writeFile("./static/"+ID_NUM+"/"+"FDA/Single measurement.txt", "\n"+Recieved_data[0]+"\t"+Recieved_data[1]+"\t"+Recieved_data[2],{flag:'a',encoding:'utf-8',mode:'0666'},  function(err) {
			   if (err) {
				   return console.error(err);
			   }					  
		});	
		}
		else if (Recieved_data[3] >0)
		{
			fs.writeFile("./static/"+ID_NUM+"/"+"FDA/Multiple measurement.txt", "\n"+Recieved_data[0]+"\t"+Recieved_data[1]+"\t"+Recieved_data[2]+"\t"+Recieved_data[3], {flag:'a',encoding:'utf-8',mode:'0666'}, function(err) {
			   if (err) {
				   return console.error(err);
			   }					  
			});

		}
	}
	//DC数据接收
	else if(Recieved_data.length == 3)
	{
		fs.writeFile("./static/"+ID_NUM+"/"+"DC/U_I_R_Data.txt", Recieved_data[0] + "\t" + Recieved_data[1] + "\t" + Recieved_data[2] + "\t" + Recieved_data[0]/Recieved_data[2], {flag:'a',encoding:'utf-8',mode:'0666'}, function(err) {
		   if (err) {
			   return console.error(err);
		   }					  
		});	
		
	}
	//TD数据接收
	else if(Recieved_data.length == 2)
	{
		fs.writeFile("./static/"+ID_NUM+"/"+"TD/Single measurement.txt", Recieved_data[0] + "\t" + Recieved_data[1], {flag:'a',encoding:'utf-8',mode:'0666'}, function(err) {
		   if (err) {
			   return console.error(err);
		   }					  
		});	
		
	}
	//Combination数据接收
	else if(Recieved_data.length == 5)
	{
		fs.writeFile("./static/"+ID_NUM+"/"+"Combination/Combination_Measurement.txt", "\n"+Recieved_data[0]+"\t"+Recieved_data[1]+"\t"+Recieved_data[2]+"\t"+Recieved_data[3], {flag:'a',encoding:'utf-8',mode:'0666'}, function(err) {
			   if (err) {
				   return console.error(err);
			   }					  
			});
		
	}
	
	}
	
	
	//io.emit('chat message', dataFIFO.shift());
  }); 
  
  socket.on('chat message', function(msg){
	io.emit('chat message', dataFIFO.shift());
  }); 
  
});




http.listen(port, function(){
  console.log('listening on *:' + port);
});







