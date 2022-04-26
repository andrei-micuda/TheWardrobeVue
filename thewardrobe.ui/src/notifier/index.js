import { message } from 'ant-design-vue';

// TODO no colors on messages icons

const notifier = {
  success: function (msg) {
    message.success({ content: () => msg });
  },
  error: function (msg) {
    message.error({
      content: () => msg,
      duration: 5,
    });
  }
}

export default notifier;