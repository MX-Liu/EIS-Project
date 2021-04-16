//导入工具包 require('node_modules里对应模块')
var gulp = require('gulp'), //本地安装gulp所用到的地方
    less = require('gulp-less'),
    minifycss = require('gulp-minify-css'),
    uglify= require('gulp-uglify'),
    watch = require('gulp-watch');
 
gulp.task('watch', function () {
    // Endless stream mode 
    watch('src/less/*.less', { ignoreInitial: false })
	       .pipe(less())
            .pipe(gulp.dest('static/dist/css'));
    watch('src/js/*.js', { ignoreInitial: false })
           .pipe(uglify())
            .pipe(gulp.dest('static/dist/js'));
});

 
gulp.task('jsmin', function () {
    //压缩src/js目录下的所有js文件
    //除了test1.js和test2.js（**匹配src/js的0个或多个子文件夹）
    gulp.src('src/js/*.js') 
        .pipe(uglify())
        .pipe(gulp.dest('static/dist/js'));
});
 
//定义一个testLess任务（自定义任务名称）
gulp.task('less', function () {
    gulp.src('src/less/*.less') //该任务针对的文件
        .pipe(less()) //该任务调用的模块
        .pipe(minifycss())
        .pipe(gulp.dest('static/dist/css')); //将会在src/css下生成index.css
});

// 图片转移
gulp.task('images', function() {
  return gulp.src('src/img/**/*')
    .pipe(gulp.dest('dist/img'));
});
 
gulp.task('default',['less', 'jsmin', 'images']); //定义默认任务 elseTask为其他任务，该示例没有定义elseTask任务
 
//gulp.task(name[, deps], fn) 定义任务  name：任务名称 deps：依赖任务名称 fn：回调函数
//gulp.src(globs[, options]) 执行任务处理的文件  globs：处理的文件路径(字符串或者字符串数组) 
//gulp.dest(path[, options]) 处理完后文件生成路径



