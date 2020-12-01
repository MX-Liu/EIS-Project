alert("xssdasd");
$(
                function () {
                    var can = $("#panel").get(0).getContext("2d");//设置为2d画布
                    var canData = {
                        labels:["a","b","c","d","e","f"],//x轴的坐标
                        datasets:[
                            {
                                fillColor:"rgba(255,0,255,0.2)",//线条下的填充色
                                strokeColor:"rgba(0,255,0,1)",//线条颜色
                                data:[1,2,3,1,2,5] //y轴对应x轴的数据

                            }

                        ]
                    };
                    
					var line = new Chart(can , {
							type: "line",
							data: canData, 
						});
		
					<!-- var line = new Chart(can).Line(canData); -->
					
                    $("#btdn1").click(
                        function () {
                            Inter=setInterval(
                                function () {
                                    $.ajax( //局部提交，只是提交一部分网页，数据通过js返回
                                        {
                                            type:"get",//ajax请求的类型
                                            url:"/static/data.py",//ajax请求的地址,动态获取数据，数据由/cgi-bin/data.py提供
                                            data:"",//请求需要携带的数据
                                            success:function (data) {
                                                line.addData(
                                                    [data["num"]],
                                                    data["now"]
                                                );
                                                var len = line.datasets[0].points.length;//获取点个数
                                                if(len > 8){
                                                    line.removeData()
                                                }
                                            },//回调函数，如果请求成功，ajax自动调用当前函数，将返回结果传递给data
                                            error:function (error) {
                                                console.log(error)
                                            }//回调函数，如果请求失败，ajax自动调用当前函数，将返回错误传递给error
                                        }
                                    )//使用ajax
                                },1000
                            )

                    $("#btdn2").click(
                        function () {
                            clearInterval(Inter);
                        }
                    )
                        }
                    )

                }
            )