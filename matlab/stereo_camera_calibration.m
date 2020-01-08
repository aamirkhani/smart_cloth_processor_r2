clear;clc;close all
figure

%%
camList = webcamlist

%%
cam1 = webcam(1)
cam2 = webcam(2)

%%

for n=1:25
    for i=5:-1:0
        subplot(3,2,[1 2]);
        pause (1);
        i
    end
    
    img1 = snapshot(cam1);
    img2 = snapshot(cam2);
    
    imwrite(img1, ['pics/cam1__' num2str(n) '.png'] )
    imwrite(img2, ['pics/cam2__' num2str(n) '.png'] )


    subplot(3,2,[4 6])
    image(img2);
    a=gca
    a.DataAspectRatio=[1 1 1]
end

%%
clear cam1 cam2