
var data_buff = [];
var express = require('express');
var path = require("path");
var fs = require("fs");
var app = require('express')();
var http = require('http').Server(app);
var io = require('socket.io')(http);
var port = process.env.PORT || 8080;



var querystring = require('querystring');

var dataFIFO = [];
var ID_NUM = [];
var Recieved_data = [];



app.use('/',express.static(path.join(__dirname,'/static')));

app.get('/', function(req, res){
  res.sendFile(__dirname + '/EIS.html');
});


io.on('connection', function(socket){
  socket.on('result', function(msg){
	
	data_buff = msg.split(":");
	
	for (i = 0; i < 16; i++) 
	{ 
		dataFIFO.push(data_buff[i]);
		Recieved_data = data_buff[i].split(",");
	if  (Recieved_data.length == 1)
	{
			ID_NUM = Recieved_data[0];
			fs.mkdir("./static/"+ID_NUM+"/",function(err){
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







