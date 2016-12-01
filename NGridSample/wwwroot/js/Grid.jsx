var GridHeader = React.createClass({

    render: function () {
        if (this.props.sorted) {
            if (this.props.sortedDesc) {
                var sortIcon = "glyphicon glyphicon-sort-by-attributes-alt pull-right";
            }
            else {
                var sortIcon = "glyphicon glyphicon-sort-by-attributes pull-right";
            }
        }
        else {
            var sortIcon = "glyphicon glyphicon-sort pull-right";
        }
        return (<th>{this.props.name}<span className={sortIcon} aria-hidden="true" /></th>)
}
});

var Grid = React.createClass({

    getInitialState: function () {

        return {
            columns: [],
            data: [],
            sortColumns: []
        };
    },

    componentDidMount: function () {

        this.serverRequest = $.post({ url: "/Home/GetData" }).success(function (result) {
            this.setState(result);
        }.bind(this));
    },
    render: function () {
        var columns = this.state.columns.map(function (item, index) {
            return (<GridHeader key={index} name={item.name} sorted={item.sorted } sortedDesc={item.sortedDesc}/>)
        });

        var columnData = this.state.columns;

        var rows = this.state.data.map(function (item, index) {
            var cols = columnData.map(function (col, index) {
                return (<td key={index}>{item[col.name.toLowerCase()]}</td>)
            });
            return (<tr key={index}>{cols}</tr>)
        });

        return (
          <div className="table-responsive">
            <table className="table table-bordered table-striped">
                <thead>
                    <tr>
                    {columns}
                </tr>
                </thead>
                <tbody>
                    {rows}
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