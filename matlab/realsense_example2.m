%Make Pipeline object to manage streaming
pipe = realsense.pipeline();
%Make Colorizer object to prettify depth output
colorizer = realsense.colorizer();
%Start streaming on an arbitrary camera with default settings
profile = pipe.start()

%Set the timeframe for streaming in seconds
        
%Wait for the frames
fs = pipe.wait_for_frames();
%Select depth frame
depth = fs.get_depth_frame();
color_frame = fs.get_color_frame();
%Colourize the depth frame 

color = colorizer.colorize(depth);
%color = colorizer.colorize(color_frame);

%Get actual data and convert into a format imshow can use
data = color.get_data();
%(Color data arrives as [R, G, B, R, G, B, ...] vector)
img = permute(reshape(data',[3,color.get_width(),color.get_height()]),[3 2 1]);
%Display the image
imshow(img)
