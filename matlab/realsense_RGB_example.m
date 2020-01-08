%Make Pipeline object to manage streaming
pipe = realsense.pipeline();
%Start streaming on an arbitrary camera with default settings
profile = pipe.start()

%Set the timeframe for streaming in seconds
tic
while toc < inf

%Wait for the frames
fs = pipe.wait_for_frames();
%Select color frame
color = fs.get_color_frame();
%Get actual data and convert into a format imshow can use
data = color.get_data();
%(Color data arrives as [R, G, B, R, G, B, ...] vector)
img = permute(reshape(data',[3,color.get_width(),color.get_height()]),[3 2 1]);
%Display the image
imshow(img)
end