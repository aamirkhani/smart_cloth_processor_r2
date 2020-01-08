% Start streaming on an arbitrary camera with default settings
profile = pipe.start();

% Get streaming device's name
dev = profile.get_device();
name = dev.get_info(realsense.camera_info.name);

% -----------------------------First frame ----------------------------------
fs = pipe.wait_for_frames();
% Select depth frame
depth = fs.get_depth_frame();
% Colorize depth frame
color = colorizer.colorize(depth);

% Get actual data and convert into a format imshow can use
% (Color data arrives as [R, G, B, R, G, B, ...] vector)
data = color.get_data();
img = permute(reshape(data', [3, color.get_width(), color.get_height()]), [3 2 1]);

% Display image
h = imshow(img);

% ---------------------------------------------------------------------

% Get frames.
while true
    fs = pipe.wait_for_frames();
    % Select depth frame
    depth = fs.get_depth_frame();
    % Colorize depth frame
    color = colorizer.colorize(depth);
    
    % Get actual data and convert into a format imshow can use
    % (Color data arrives as [R, G, B, R, G, B, ...] vector)
    data = color.get_data();
    img = permute(reshape(data', [3, color.get_width(), color.get_height()]), [3 2 1]);
    
    % Display image
    set(h, 'CData', img);  % faster than imshow inside a loop
    drawnow;
end

% Stop streaming
pipe.stop();