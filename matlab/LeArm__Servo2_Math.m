clear;clc;close all
f=figure
f.Position = [1510 42 1050 1314]

%%
dataA=csvread('LeArm_data__servo2__R2.txt');
dataB=csvread('LeArm_data__servo2.txt');
dataL=csvread('LeArm_data__LeapMotion___R2.txt')

data=dataA;

i=1
len=6; S = data(:,i:(i-1 +len)); i=i+len;
data=data( S(:,1)==1, :);


%%

i=1
len=6; S = data(:,i:(i-1 +len)); i=i+len;

len=3; S1Ap       = data(:,i:(i-1 +len)); i=i+len;
len=4; S1Aq       = data(:,i:(i-1 +len)); i=i+len;
len=3; S1Aright   = data(:,i:(i-1 +len)); i=i+len;
len=3; S1Aup      = data(:,i:(i-1 +len)); i=i+len;
len=3; S1Aforward = data(:,i:(i-1 +len)); i=i+len;

len=3; S1Bp       = data(:,i:(i-1 +len)); i=i+len;
len=4; S1Bq       = data(:,i:(i-1 +len)); i=i+len;
len=3; S1Bright   = data(:,i:(i-1 +len)); i=i+len;
len=3; S1Bup      = data(:,i:(i-1 +len)); i=i+len;
len=3; S1Bforward = data(:,i:(i-1 +len)); i=i+len;

len=3; S2p       = data(:,i:(i-1 +len)); i=i+len;
len=4; S2q       = data(:,i:(i-1 +len)); i=i+len;
len=3; S2right   = data(:,i:(i-1 +len)); i=i+len;
len=3; S2up      = data(:,i:(i-1 +len)); i=i+len;
len=3; S2forward = data(:,i:(i-1 +len)); i=i+len;


LI=dataL(:,1:3)
LT=dataL(:,4:6)

clear i len
%%
S1p=(S1Ap + S1Bp)/2

%% visualize all

i=1500
v=[S1p];
plot3(v(:,1),v(:,2),v(:,3),'.')
set(gca, 'YDir','reverse')
grid on
xlabel('X')
ylabel('Y')
zlabel('Z')
a=gca
a.DataAspectRatio=[1 1 1]
%view(77,17)
view(89.8118 , -0.1264)



Y=[0 1 0]

S2=S(:,2)
s2right = S2right/(norm(S2right))
hold on
%v=[S2p S2right];
%quiver3(v(:,1),v(:,2),v(:,3),v(:,4),v(:,5),v(:,6),200)
v=[ S2p S2up];
quiver3(v(:,1),v(:,2),v(:,3),v(:,4),v(:,5),v(:,6),200)

v=[ S2p S2forward];
quiver3(v(:,1),v(:,2),v(:,3),v(:,4),v(:,5),v(:,6),200)


v=[ S2p S2forward.*S2];
quiver3(v(:,1),v(:,2),v(:,3),v(:,4),v(:,5),v(:,6),200)

v=[ S2p repmat(Y,length(S2p),1)]
quiver3(v(:,1),v(:,2),v(:,3),v(:,4),v(:,5),v(:,6),500)

TRANSLATE=mean([S2p;S1p])

scale=430
LI_=((LI-mean(LI)).*scale)+TRANSLATE
LT_=((LT-mean(LT)).*scale)+TRANSLATE
v=[LI_]
%plot3(v(:,1),v(:,2),v(:,3),'b.')
v=[LT_]
%plot3(v(:,1),v(:,2),v(:,3),'g.')


index=250
I=LI_(index,:)
T=LT_(index,:)
v=[I]
plot3(v(:,1),v(:,2),v(:,3),'bo', 'MarkerFaceColor','b')
v=[T]
plot3(v(:,1),v(:,2),v(:,3),'go')

v=[(I+T)/2 Y]
quiver3(v(:,1),v(:,2),v(:,3),v(:,4),v(:,5),v(:,6),20)


hold off




%% visualize all
