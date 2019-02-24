import * as React from "react";

const SpaRoot = () => {
  return (
    <div className="jumbotron">
      <h1>Hello from a React SPA!</h1>
      <p className="alert alert-success">
        This div is produced by a React app located under
        (AspNetBase.Presentation.Spa/Client).
        <br />
        The React app was exported by
        Webpack from the SPA project into the APP project.
      </p>
    </div>
  );
};

export default SpaRoot;
