import random
import time
import json

now = time.strftime("%H:%M:%S",time.localtime())

num = random.randint(1,10)

result = {"now":now,"num":num}

json_result = json.dumps(result)  # 字典转换为json数据

print("content-type:application/json")  # 返回响应的头部，具体描述的要返回的内容类型，在cgi当中用print进行返回
print("\n")  # 返回头部结束
print(json_result)  # 返回响应的body
