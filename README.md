# drewy
4.6 AGI House Aerospace hackathon


## Set up
If [uv](https://github.com/astral-sh/uv) is not installed: 
```
pip install uv
```

Create a virtual env (basically following the uv GitHub doc):
```
uv venv  # Create a virtual environment at .venv.
```
Activate the virtual environment. 
```
# On macOS and Linux.
source .venv/bin/activate

# On Windows.
.venv\Scripts\activate
```

Create a .streamlit at the project dir, then put OpenAI API Key in the secrets.toml
```
OPENAI_API_KEY=""
```


## Appendix

How I started installing all of those:
```
uv pip install streamlit
```
