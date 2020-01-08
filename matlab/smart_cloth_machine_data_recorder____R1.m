clear; close all; clc
f=figure
f.Position =   [1503          42        1057        1314]





%Make Pipeline object to manage streaming
pipe = realsense.pipeline();
%Make Colorizer object to prettify depth output
colorizer = realsense.colorizer();
%Start streaming on an arbitrary camera with default settings
profile = pipe.start()

%Set the timeframe for streaming in seconds

%%
fs = pipe.wait_for_frames();
depth = fs.get_depth_frame();
color = colorizer.colorize(depth);
data = color.get_data();
D = permute(reshape(data',[3,color.get_width(),color.get_height()]),[3 2 1]);


color = fs.get_color_frame();
%Get actual data and convert into a format imshow can use
data = color.get_data();
%(Color data arrives as [R, G, B, R, G, B, ...] vector)
RGB = permute(reshape(data',[3,color.get_width(),color.get_height()]),[3 2 1]);

%  [299.4950 115.3744]
%  [945.3241 598.3894]
D_ = D(115:598,299:945);

%
clc
RGB_ = imresize(RGB,[size(D,1) size(D,2)]);
size(RGB)
size(RGB_)
size(D)
[size(D,2)/2-size(RGB,2)/2  size(D,1)/2-size(RGB,1)/2
size(D,2)/2+size(RGB,2)/2  size(D,1)/2+size(RGB,1)/2]

D__ = D(size(D,1)/2-size(RGB,1)/2:size(D,1)/2+size(RGB,1)/2-1   , size(D,2)/2-size(RGB,2)/2:size(D,2)/2+size(RGB,2)/2-1)
size(D__)

%

subplot(3,1,1)
image(D)
a=gca
a.DataAspectRatio=[1 1 1]

subplot(3,1,2)
image(RGB)
a=gca
a.DataAspectRatio=[1 1 1]

subplot(3,1,3)
image(D__)
a=gca
a.DataAspectRatio=[1 1 1]


size(D_)
size(RGB)

%
W=size(D__,2)
H=size(D__,1)
[X1,Y1]=meshgrid(1:W,1:H);

scale=0.3
Z1=reshape(double(D__),H,W) * scale;
%Z1=zeros(H,W);

X1=X1(:);
Y1=Y1(:);
Z1=Z1(:);


C1=reshape(RGB,H*W,3);
C1(Z1==0,:)=zeros( nnz(Z1==0),3)

pcshow([X1 Y1 Z1],C1)
a=gca
a.DataAspectRatio = [1 1 1]






