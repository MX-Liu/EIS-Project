
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
	
	for (i = 0; i < 1; i++) 
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
					console.log(ID_NUM + " successfully created");
					fs.mkdir("./static/"+ID_NUM+"/"+"FDA",function(err){
					if (err){			
					}
					else
					{
						//创建以FDA为名的子文件夹
						console.log("FDA successfully created");	
						//创建Single  Measurement 的txt文档
						fs.writeFile("./static/" + ID_NUM + "/" + "FDA/0Single measurement.txt", "Fre" + "\t" + "Mag" + "\t" + "Pha" + "\t",  function(err) {
						   if (err) {
							   return console.error(err);
						   }					  
						});		
						//创建multiple measurement 的txt文档
						fs.writeFile("./static/" + ID_NUM + "/" + "FDA/0Multiple measurement.txt", "Fre" + "\t" + "Mag" + "\t" + "Pha" + "\t" + "Times" + "\t",  function(err) {
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
						console.log("TD successfully created");	
						//创建Single  Measurement 的txt文档
						fs.writeFile("./static/" + ID_NUM + "/" + "TD/0Single measurement.txt", "Fre" + "\t" + "Mag" + "\t" + "Pha" + "\t" + "Times" + "\t",  function(err) {
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
						console.log("DC successfully created");		
						//创建U_I_R_Data的txt文档
						fs.writeFile("./static/" + ID_NUM + "/" + "DC/0U_I_R_Data.txt", "Time" + "\t" + "Voltage" + "\t" + "Current" + "\t" + "Resistance" + "\t",  function(err) {
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
						console.log("Combination successfully created。");	
						//创建U_I_R_Data的txt文档
						fs.writeFile("./static/" + ID_NUM + "/" + "Combination/0Combination_Measurement.txt", "Fre" + "\t" + "Mag" + "\t" + "Pha" + "\t" + "Times" + "\t",  function(err) {
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
	else if (Recieved_data[0] == "AA" &&  Recieved_data[1] == "FF" &&  Recieved_data[2] == "FF" && Recieved_data[3] == "AA" &&  Recieved_data[4] == "1")
	{
		
		if (Recieved_data[8] == 0)
		{
			console.log('FDA Single data :' + Recieved_data[5] + ',' +Recieved_data[6] + ',' + Recieved_data[7]+ ',' + Recieved_data[8] );
			fs.writeFile("./static/" + ID_NUM + "/" + "FDA/"+ Recieved_data[9]+"Single measurement.txt", "\n" + Recieved_data[5] + "\t" + Recieved_data[6] + "\t" + Recieved_data[7] + "\t",{flag:'a',encoding:'utf-8',mode:'0666'},  function(err) {
			   if (err) {
				   return console.error(err);
			   }					  
		});	
		}
		else if (Recieved_data[8] >0)
		{
			console.log('FDA Mutiple data :' + Recieved_data[5] + ',' +Recieved_data[6] + ',' + Recieved_data[7]+ ',' + Recieved_data[8] );
			fs.writeFile("./static/" + ID_NUM + "/" + "FDA/"+ Recieved_data[9]+ "Multiple measurement.txt", "\n" + Recieved_data[5] + "\t" + Recieved_data[6] + "\t" + Recieved_data[7] + "\t" + Recieved_data[8] + "\t", {flag:'a',encoding:'utf-8',mode:'0666'}, function(err) {
			   if (err) {
				   return console.error(err);
			   }					  
			});

		}
	}
	//DC数据接收
	else if (Recieved_data[0] == "AA" &&  Recieved_data[1] == "FF" &&  Recieved_data[2] == "FF" && Recieved_data[3] == "AA" && Recieved_data[4] == "3")
	{
		console.log('DC data :' + Recieved_data[5] + ',' + Recieved_data[6] + ',' +Recieved_data[7] );
		fs.writeFile("./static/" + ID_NUM + "/" + "DC"+ Recieved_data[8] +"/U_I_R_Data.txt", "\n" + Recieved_data[5] + "\t" + Recieved_data[6] + "\t" + Recieved_data[7] + "\t" + Recieved_data[6] / Recieved_data[7] + "\t", {flag:'a',encoding:'utf-8',mode:'0666'}, function(err) {
		   if (err) {
			   return console.error(err);
		   }					  
		});	
		
	}
	//TD数据接收
	else if (Recieved_data[0] == "AA" &&  Recieved_data[1] == "FF" &&  Recieved_data[2] == "FF" && Recieved_data[3] == "AA"  && Recieved_data[4] == "5")
	{
		console.log('TD data :' + Recieved_data[5] + ',' + Recieved_data[6] + ',' + Recieved_data[7]+ ',' + Recieved_data[8]+ ',' + Recieved_data[9] );
		fs.writeFile("./static/" + ID_NUM + "/" + "TD/"+ Recieved_data[9] + "Single measurement.txt", "\n" + Recieved_data[5] + "\t" + Recieved_data[6] + "\t" + Recieved_data[7] + "\t" + Recieved_data[8] + "\t", {flag:'a',encoding:'utf-8',mode:'0666'}, function(err) {
		   if (err) {
			   return console.error(err);
		   }					  
		});	
		
	}
	//Combination数据接收
	else if (Recieved_data[0] == "AA" &&  Recieved_data[1] == "FF" &&  Recieved_data[2] == "FF" && Recieved_data[3] == "AA" && Recieved_data[4] == "4")
	{
		console.log('Comb data :' + Recieved_data[5] +','+Recieved_data[6] + ',' + Recieved_data[7]+ ',' + Recieved_data[8] );
		fs.writeFile("./static/" + ID_NUM + "/"  + "Combination/" + Recieved_data[9] + "Combination_Measurement.txt", "\n" + Recieved_data[5] + "\t" + Recieved_data[6] + "\t" + Recieved_data[7] + "\t" + Recieved_data[8] + "\t", {flag:'a',encoding:'utf-8',mode:'0666'}, function(err) {
			   if (err) {
				   return console.error(err);
			   }					  
			});
		
	}
	
	}

  }); 
  
  socket.on('chat message', function(msg){
	io.emit('chat message', dataFIFO.shift());
  }); 
  
});




http.listen(port, function(){
  console.log('listening on *:' + port);
});







