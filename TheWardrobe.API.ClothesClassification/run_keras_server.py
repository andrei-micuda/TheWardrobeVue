# import the necessary packages
import io
import tensorflow as tf
from PIL import Image
import numpy as np
from flask import Flask, jsonify, request
import cc_model

# initialize our Flask application and the Keras model
app = Flask(__name__)


@app.route("/predict", methods=["POST"])
def predict():
    """POST REST endpoint for clothes classification."""
    # initialize the data dictionary that will be returned from the
    # view
    data = {"success": False}

    # ensure an image was properly uploaded to our endpoint
    if request.method == "POST":
        if request.files.get("image"):
            # read the image in PIL format
            image = request.files["image"].read()
            image = Image.open(io.BytesIO(image))
            min_score = float(request.args.get('min_score')) * 100

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
    app.run()
