clear; close all; clc



pipe = realsense.pipeline();
colorizer = realsense.colorizer();
profile = pipe.start()


%%
fs = pipe.wait_for_frames();

points = pointcloud.calculate(depth);
vertices = points.get_vertices();
size(vertices)
X = vertices(:,1,1);
Y = vertices(:,2,1);
Z = vertices(:,3,1);
39.99
    plot3(X,Z,-Y,'.');

% Realsense__Depth 1280 x 
depth = fs.get_depth_frame();
color = colorizer.colorize(depth);
data = color.get_data();
D = permute(reshape(data',[3,color.get_width(),color.get_height()]),[3 2 1]);

% Realsense__RGB 640x480
color = fs.get_color_frame();
data = color.get_data();
RGB = permute(reshape(data',[3,color.get_width(),color.get_height()]),[3 2 1]);

imwrite(D,'D.png')
imwrite(RGB,'RGB.png')

