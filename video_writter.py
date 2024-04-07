import cv2
import os
def video_writter(input_dir, outout_dir):
# Specify the path to the directory containing the pictures
    image_folder = input_dir

    # Get a list of image file names
    images = [img for img in os.listdir(image_folder) if img.endswith(".png")]

    # Sort the images based on their names (assuming they are named in the desired sequence)
    images.sort()

    # Get the dimensions of the first image
    frame = cv2.imread(os.path.join(image_folder, images[0]))
    height, width, layers = frame.shape

    # Specify the video output settings
    video_name = 'outout_dir'
    fourcc = cv2.VideoWriter_fourcc(*'XVID')
    fps = 2  # Frames per second
    video = cv2.VideoWriter(video_name, fourcc, fps, (width, height))

    # Iterate through the images and write them to the video
    for image in images:
        frame = cv2.imread(os.path.join(image_folder, image))
        video.write(frame)

    # Release the video object
    video.release()