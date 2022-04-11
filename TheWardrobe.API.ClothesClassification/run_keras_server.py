# import the necessary packages
from io import BytesIO
import requests
import tensorflow as tf
from PIL import Image
import numpy as np
from flask import Flask, jsonify, request
import cc_model

# initialize our Flask application and the Keras model
app = Flask(__name__)

# load image from url to NumPy array


def load_image(url):
    res = requests.get(url)
    if res.status_code == 200:
        img_arr = Image.open(BytesIO(res.content))
        return img_arr
    else:
        return None


@app.route("/predict", methods=["POST"])
def predict():
    """POST REST endpoint for clothes classification."""
    # initialize the data dictionary that will be returned from the
    # view
    data = {"success": False}

    # ensure an image was properly uploaded to our endpoint
    if request.method == "POST":
        req_body = request.json
        resource_url = req_body["resourceUrl"]
        min_score = req_body["minScore"]

        image = load_image(resource_url)
        if image is not None:
            predictions = cc_model.predict(image)
            curr_score = 0
            data["predictions"] = []
            for (cat, score) in predictions:
                curr_score += score
                data["predictions"].append((cat, score))
                if(curr_score >= min_score):
                    break

            # indicate the total score
            data["score"] = curr_score
            # indicate that the request was a success
            data["success"] = True

        # return the data dictionary as a JSON response
    return jsonify(data)


# if this is the main thread of execution first load the model and
# then start the server
if __name__ == "__main__":
    print(("* Loading Keras model and Flask starting server..."
           "please wait until server has fully started"))
    cc_model.load()
    app.run(host='0.0.0.0')
