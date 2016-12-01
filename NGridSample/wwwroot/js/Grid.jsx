var GridHeader = React.createClass({
    setSort: function () {
        this.props.toggleSort(this.props.name)
    },
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
        return (<th>{this.props.name}<a onClick={this.setSort }><span className={sortIcon} aria-hidden="true" /></a></th>)
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

    toggleSort: function (columnName) {
        this.update({ sortColumns: [{ column: columnName, sortDesc: false }] });
    },

    update: function(props) {
        this.serverRequest = $.post({ url: "/Home/GetData" }, props).success(function (result) {
            this.setState(result);
        }.bind(this));
    },

    componentDidMount: function () {
        this.update({});
    },
    render: function () {
        var columns = this.state.columns.map(function (item, index) {
            return (<GridHeader key={index} name={item.name} sorted={item.sorted} sortedDesc={item.sortedDesc} toggleSort={this.toggleSort }/>)
        }, this);

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