from openai import OpenAI
import streamlit as st
import time

st.title("Drewy")
st.markdown("Drewy let you control swarms of drones in a simulated environment to accomplish tasks \
            like drone formation and multi-angle drone photography. ")

client = OpenAI(api_key=st.secrets["OPENAI_API_KEY"])

if "openai_model" not in st.session_state:
    st.session_state["openai_model"] = "gpt-3.5-turbo"

if "messages" not in st.session_state:
    st.session_state.messages = []

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
        assistant = client.beta.assistants.retrieve("asst_IntchtGliaxEIXtJke80Whzw")
        thread = client.beta.threads.create()
        message = client.beta.threads.messages.create(
            thread_id=thread.id,
            role="user",
            content=prompt
        )
        run = client.beta.threads.runs.create_and_poll(
            thread_id=thread.id,
            assistant_id=assistant.id,
            instructions=prompt
        )

        # with client.beta.threads.runs.stream(
        #     thread_id=thread.id,
        #     assistant_id=assistant.id,
        #     instructions="Please address the user as Jane Doe. The user has a premium account.",
        #     event_handler=EventHandler(),
        #     ) as stream:
        #     # stream.until_done()
            

        # stream = client.chat.completions.create(
        #     model=st.session_state["openai_model"],
        #     messages=[
        #         {"role": m["role"], "content": m["content"]}
        #         for m in st.session_state.messages
        #     ],
        #     stream=True,
        # )
        fake_response = st.write_stream(response_generator())
        # response = st.write_stream(stream)

        if run.status == 'completed': 
            messages = client.beta.threads.messages.list(
                thread_id=thread.id
            )
            st.markdown(messages.data[0].content[0])
        else:
            st.markdown(run.status)

        VIDEO_URL = "https://www.xiangyi.li/titanic.mp4"
        st.video(VIDEO_URL)
    st.session_state.messages.append({"role": "assistant", "content": messages.data[0].content})