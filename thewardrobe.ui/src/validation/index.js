import { extend } from "vee-validate";
import { required, email } from "vee-validate/dist/rules";

extend("email", {
  ...email,
  message: "Please provide a valid email address.",
});

extend("required", {
  ...required,
  message: "This field is required.",
});

extend("passwordMatch", {
  params: ["target"],
  validate(value, { target }) {
    return value === target;
  },
  message: "Passwords do not match.",
});
