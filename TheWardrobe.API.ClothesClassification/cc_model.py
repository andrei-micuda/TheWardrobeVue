import tensorflow as tf
import tensorflow_hub as hub
import numpy as np


class MyBiTModel(tf.keras.Model):
    """BiT with a new head."""

    def __init__(self, num_classes, module):
        super().__init__()

        self.num_classes = num_classes
        self.head = tf.keras.layers.Dense(
            num_classes, kernel_initializer='zeros')
        self.bit_model = module

    def call(self, images):
        # No need to cut head off since we are using feature extractor model
        bit_embedding = self.bit_model(images)
        return self.head(bit_embedding)


def load():
    # Load model into KerasLayer
    global MODEL
    model_url = "https://tfhub.dev/google/experts/bit/r50x1/in21k/clothing/1"
    module = hub.KerasLayer(model_url)
    MODEL = MyBiTModel(num_classes=50, module=module)
    MODEL.load_weights("weights/best_model.ckpt")


def preprocess_image(image):
    """Preprocess the image before feeding it to the model."""
    # if the image mode is not RGB, convert it
    if image.mode != "RGB":
        image = image.convert("RGB")

    image = np.array(image)
    # reshape into shape [batch_size, height, width, num_channels]
    img_reshaped = tf.reshape(
        image, [1, image.shape[0], image.shape[1], image.shape[2]])
    # Use `convert_image_dtype` to convert to floats in the [0,1] range.
    image = tf.image.convert_image_dtype(img_reshaped, tf.float32)
    return image


def predict(image):
    image = preprocess_image(image)
    preds = MODEL(image)
    return process_preds(preds)


def process_preds(logits):
    if len(logits.shape) > 1:
        logits = tf.reshape(logits, [-1])

    classes = []
    scores = []
    logits_max = np.max(logits)
    softmax_denominator = np.sum(np.exp(logits - logits_max))
    for index, j in enumerate(np.argsort(logits)[::][::-1]):
        score = 1.0 / (1.0 + np.exp(-logits[j]))
        classes.append(CATEGORIES[j])
        scores.append(
            np.exp(logits[j] - logits_max) / softmax_denominator * 100)

    return list(zip(classes, scores))


MODEL = None

CATEGORIES = [
    'Anorak', 'Blazer', 'Blouse', 'Bomber', 'Button-Down', 'Cardigan', 'Flannel', 'Halter', 'Henley', 'Hoodie', 'Jacket', 'Jersey', 'Parka', 'Peacoat', 'Poncho', 'Sweater', 'Tank', 'Tee', 'Top', 'Turtleneck', 'Capris', 'Chinos', 'Culottes', 'Cutoffs', 'Gauchos', 'Jeans', 'Jeggings', 'Jodhpurs', 'Joggers', 'Leggings', 'Sarong', 'Shorts', 'Skirt', 'Sweatpants', 'Sweatshorts', 'Trunks', 'Caftan', 'Cape', 'Coat', 'Coverup', 'Dress', 'Jumpsuit', 'Kaftan', 'Kimono', 'Nightdress', 'Onesie', 'Robe', 'Romper', 'Shirtdress', 'Sundress']
