var Grid = React.createClass({
    render: function() {
        return (
          <div className="table-responsive">
            <table className="table table-bordered table-striped">
                <thead>
                    <tr>
                    <th>Column1</th><th>Column2</th>
                </tr>
                </thead>
                <tbody>
                    <tr><td>item1</td><td>item2</td></tr>
                </tbody>
            </table>
          </div>
      );
    }
});
ReactDOM.render(
  <Grid />,
  document.getElementById('content')
);