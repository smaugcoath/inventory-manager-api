namespace InventoryManager.WebApi.Mvc.Controllers
{
    using AutoMapper;
    using InventoryManager.WebApi.Business.Models;
    using InventoryManager.WebApi.Business.Services.ItemService;
    using InventoryManager.WebApi.Mvc.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;


    /// <summary>
    /// The inventory items resource.
    /// </summary>
    [ApiController]
    // JWT bearer authentication is wired up, but no identity provider ships with the sample,
    // so [Authorize] and the per-action policies below are left in place as documentation.
    //[Authorize]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsController"/> class.
        /// </summary>
        /// <param name="itemService">The service that owns the inventory logic.</param>
        /// <param name="mapper">Maps business models to the public API models.</param>
        /// <param name="logger">The logger for this controller.</param>
        public ItemsController(
            IItemService itemService,
            IMapper mapper,
            ILogger<ItemsController> logger)
        {
            _itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Returns the inventory item with the given name.
        /// </summary>
        /// <param name="name">The name that identifies the item.</param>
        /// <returns>The requested item.</returns>
        [HttpGet("{name}")]
        //[Authorize("ReadPolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAsyncResponse>> GetAsync(string name)
        {
            var item = await _itemService.GetAsync(name);

            var result = _mapper.Map<GetAsyncResponse>(item);

            return Ok(result);
        }

        /// <summary>
        /// Adds an item to the inventory and schedules its expiry.
        /// </summary>
        /// <param name="request">The item to add.</param>
        /// <returns>The item as it was stored, with a Location header pointing at it.</returns>
        // POST rather than PUT because the request targets the collection URI: RFC 7231 defines
        // PUT against a target URI that identifies the resource being replaced, and here the
        // identifier travels in the body. The item name is still a natural key, so replaying the
        // same request is answered with 409 Conflict instead of creating a duplicate.
        [HttpPost]
        //[Authorize("CreatePolicy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<AddAsyncResponse>> AddAsync(AddAsyncRequest request)
        {
            var item = _mapper.Map<Item>(request);
            var itemAdded = await _itemService.AddAsync(item);
            var response = _mapper.Map<AddAsyncResponse>(itemAdded);

            // Url.ActionLink is used instead of CreatedAtAction: under the attribute routing
            // set up here, CreatedAtAction and CreatedAtRoute fail to resolve the GET route
            // even with the Async suffix preserved in action names.
            var locationHeader = Url.ActionLink(nameof(GetAsync), values: new { name = item.Name });

            return Created(locationHeader, response);
        }

        /// <summary>
        /// Removes the inventory item with the given name.
        /// </summary>
        /// <param name="name">The name that identifies the item.</param>
        /// <returns>The item that was removed.</returns>
        [HttpDelete("{name:required:maxlength(100)}")]
        //[Authorize("DeletePolicy")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DeleteAsyncResponse>> DeleteAsync(string name)
        {
            var item = await _itemService.DeleteAsync(name);
            var response = _mapper.Map<DeleteAsyncResponse>(item);

            return Ok(response);
        }
    }
}
