from openai import OpenAI
import streamlit as st
import time

st.title("Drewy")
st.markdown("Choreography, photography with drone swarms. ")

client = OpenAI(api_key=st.secrets["OPENAI_API_KEY"])

if "openai_model" not in st.session_state:
    st.session_state["openai_model"] = "gpt-3.5-turbo"

with open("instructions.txt", "r") as file:
    content_in_file = file.read()

if "messages" not in st.session_state:
    st.session_state.messages = [{"role": "system", "content": content_in_file}]

for message in st.session_state.messages:
    with st.chat_message(message["role"]):
        st.markdown(message["content"])

def response_generator():
    response = "Please wait for some time until the drone view is generated"
    for word in response.split():
        yield word + " "
        time.sleep(0.05)



if prompt := st.chat_input("Please enter your instructions"):

    st.session_state.messages.append({"role": "user", "content": prompt})
    with st.chat_message("user"):
        st.markdown(prompt)

    with st.chat_message("assistant"):
        stream = client.chat.completions.create(
            model=st.session_state["openai_model"],
            messages=[
                {"role": m["role"], "content": m["content"]}
                for m in st.session_state.messages
            ],
            stream=True,
        )
        fake_response = st.write_stream(stream)
        response = st.write_stream(response_generator())
        VIDEO_URL = "https://www.xiangyi.li/output1.mp4"
        st.video(VIDEO_URL)
    st.session_state.messages.append({"role": "assistant", "content": response})