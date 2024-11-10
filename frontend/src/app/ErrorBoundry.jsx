import { Component } from "react";
import { toast } from "react-toastify";
import PropTypes from "prop-types";

class ErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, errorMessage: "" };
    this.handlePromiseRejection = this.handlePromiseRejection.bind(this);
  }

  componentDidMount() {
    window.onunhandledrejection = this.handlePromiseRejection;
  }

  componentWillUnmount() {
    window.onerror = null;
    window.onunhandledrejection = null;
  }

  handlePromiseRejection(event) {
    const error = event.reason;
    this.setState({ hasError: true, errorMessage: error.toString() });
    toast.error(error.toString());
  }

  render() {
    if (this.state.hasError) {
      return null;
    }

    return this.props.children;
  }
}

ErrorBoundary.propTypes = {
  children: PropTypes.node.isRequired,
};

export default ErrorBoundary;
