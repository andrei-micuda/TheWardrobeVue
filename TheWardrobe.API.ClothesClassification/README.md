# Initialization

1. `python3 -m venv --system-site-packages ./venv`
2. `source ./venv/bin/activate`
3. `python -m pip install --upgrade pip`
4. `python -m pip install --upgrade tensorflow`
5. `python -c "import tensorflow as tf; print(tf.__version__)"` (should return something like `2.8.0`)
6. `python -m pip install --upgrade Flask`
7. `python -m pip install --upgrade tensorflow_hub`
8. `python run_keras_server.py`